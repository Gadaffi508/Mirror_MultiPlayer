using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mirror
{
    public class WeaponSpawner : NetworkBehaviour
    {
        [SerializeField] private Transform _weaponHolder;

        [SerializeField] private GameObject _swordPrefab;

        [SyncVar] private NetworkIdentity _spawnedWeapon;

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            Vector2 offset = Random.insideUnitCircle * 3f;
            transform.position += new Vector3(offset.x,offset.y,0f);
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
}
