using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Charboa
{
    namespace Utilities
    {      
        public class Delay
            {               
                public static Delay Set(float time, Action callback)    
                {
                    GameObject go = new GameObject("Delay", typeof(MonoBehaviourHook));

                    Delay timer = new Delay(time, callback, go);

                    go.GetComponent<MonoBehaviourHook>().OnUpdate = timer.Update;

                    return timer;
                }

                private Action _callback;  
                private float _timer;
                private GameObject _go;
                private Delay(float time, Action callback, GameObject go)
                {
                    _timer = time;
                    _callback = callback;
                    _go = go;
                    _callback += DestryoSelf;
                }

                public void Update()
                {
                    if (_timer > 0)
                    {
                        _timer -= Time.deltaTime;

                        if (IsTimerCompleted())
                        {
                            _callback();
                        }
                    }
                }

                private bool IsTimerCompleted()
                {
                    return _timer <= 0f;
                }

                private void DestryoSelf()
                {
                    _callback -= DestryoSelf;
                    UnityEngine.Object.Destroy(_go);
                }
            }
    }
}
