using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TetraUtils
{
    public class Timespan
    {
        public float time { get; private set; }
        public bool isRunning { get; private set; }

        private Action<float> updater;
        private List<Action> startEv = new();
        private List<Action> stopEv = new();
        public event Action OnStart;
        public event Action OnStop;

        public Timespan(Action<float> updater, Action startEvent = null, Action stopEvent = null)
        {
            this.updater = updater;
            startEv.Add(startEvent);
            stopEv.Add(stopEvent);

            if (startEvent != null)
                startEvent += StartInternal;
            if (startEvent != null)
                stopEvent += StopInternal;
        }
        ~Timespan()
        {
            for (int i = 0; i < startEv.Count; i++)
            {
                var ev = startEv[i];
                ev -= StartInternal;
            }
            for (int i = 0; i < stopEv.Count; i++)
            {
                var ev = stopEv[i];
                ev -= StopInternal;
            }
            updater -= UpdateInternal;
            OnStart = null;
            OnStop = null;
        }
        public virtual void Update() { }
        void UpdateInternal(float deltaTime)
        {
            time += deltaTime;
            Update();
        }

        void StartInternal()
        {
            if (!isRunning)
            {
                isRunning = true;
                updater += UpdateInternal;
            }
        }
        void StopInternal()
        {
            if (isRunning)
            {
                isRunning = false;
                updater -= UpdateInternal;
            }
        }
        public void AddStartListener(Action l) => OnStart += l;
        public void RemoveStartListener(Action l) => OnStart -= l;
        public void AddStopListener(Action l) => OnStop += l;
        public void RemoveStopListener(Action l) => OnStop -= l;
    }
}
