using System;
using System.Linq;
using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        [SerializeField] string _id;

        public static DontDestroyOnLoad Get(string id)
        {
            var instances = FindObjectsOfType<DontDestroyOnLoad>();
            return instances.FirstOrDefault(i => i._id == id);
        }

        [SerializeField] static bool _flag = false;
        void Awake()
        {
            if (!_flag)
            {
                _flag = true;
            }
            else
            {
                Destroy(this.gameObject);
            }

            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }

            var instance = Get(_id);

            if (instance != null && instance != this)
            {
                Destroy(instance.gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}