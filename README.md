# Unity Mirror Örnek Projeleri
Bu repo, Unity'de Mirror kütüphanesini kullanarak gerçekleştirdiğim çeşitli ağ programlama örneklerini içermektedir. Aşağıda projede yer alan başlıca bileşenler ve kod örnekleri bulunmaktadır.

## 1. İçindekiler
- Kurulum
- Projeler
- Renk Değiştirme
- SyncVar Kullanımı
- Silah Spawn Etme
- Katkıda Bulunma
- Lisans

## 2. Kurulum

Bu projeyi bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

- Unity Hub ile Unity'yi kurun (Unity 2020.3 veya daha yeni bir sürüm önerilir).
- Bu repoyu klonlayın:
```
git clone https://github.com/kullaniciadi/UnityMirrorExamples.git
```
-Projeyi Unity ile açın.
-Mirror kütüphanesini yükleyin: Window > Package Manager menüsünden Mirror'ı ekleyin.

## 3. Projeler

### - Renk Değiştirme
ChangeColor sınıfı, oyuncunun klavyeden C tuşuna bastığında rastgele bir renk seçerek tüm istemcilerde güncellemesini sağlar.

```
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Mirror;

public class ChangeColor : NetworkBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = Vector3.zero + (Random.insideUnitSphere * 3f);
    }

    private void Update()
    {
        if(!base.isOwned) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            float r = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            _spriteRenderer.color = new Color(r, g, b, 1f);
            CmdUpdateColor(_spriteRenderer.color);
        }
    }

    [Command]
    void CmdUpdateColor(Color c)
    {
        _spriteRenderer.color = c;
        RpcUpdateColor(c);
    }
    
    [ClientRpc]
    void RpcUpdateColor(Color c)
    {
        if(base.isServer) return;
        if(base.authority) return;

        _spriteRenderer.color = c;
    }
}

```

### - SyncVar Kullanımı
SyncVarDemo sınıfı, sunucu tarafında belirli aralıklarla rastgele renkler oluşturur ve bu renkleri istemcilerde senkronize eder.

```
using UnityEngine;
using Mirror;
using System.Collections;

public class SyncVarDemo : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor))]
    private Color32 _color = Color.red;

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(__RandomizeColor());
    }

    void SetColor(Color32 oldCOlor, Color32 newColor)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = newColor;
    }

    IEnumerator __RandomizeColor()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        while (true)
        {
            yield return wait;
            _color = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f, 1f, 1f);
        }
    }
}

```

### - Silah Spawn Etme
WeaponSpawner sınıfı, oyuncu S tuşuna bastığında bir silah oluşturur ve bu silahın konumunu oyuncunun konumuna göre günceller.
```
using UnityEngine;
using Mirror;

public class WeaponSpawner : NetworkBehaviour
{
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private GameObject _swordPrefab;
    [SyncVar] private NetworkIdentity _spawnedWeapon;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Vector2 offset = Random.insideUnitCircle * 3f;
        transform.position += new Vector3(offset.x, offset.y, 0f);
    }

    private void Update()
    {
        if (_spawnedWeapon != null)
            _spawnedWeapon.transform.position = _weaponHolder.position;

        if (base.authority)
        {
            float hor = Input.GetAxis("Horizontal");
            transform.position += new Vector3(hor * Time.deltaTime * 3f, 0f, 0f);

            if (Input.GetKeyDown(KeyCode.S))
                CmdSpawnWeapon();
        }
    }

    [Command]
    void CmdSpawnWeapon()
    {
        if(_spawnedWeapon != null)
            return;

        GameObject go = Instantiate(_swordPrefab);
        NetworkServer.Spawn(go);
        _spawnedWeapon = go.GetComponent<NetworkIdentity>();
    }
}

```

Katkıda Bulunma
Katkılarınızı memnuniyetle karşılıyoruz! Lütfen bir pull request oluşturun veya bir issue açın.

Lisans
Bu proje MIT Lisansı ile lisanslanmıştır. Daha fazla bilgi için LICENSE dosyasına bakın.
