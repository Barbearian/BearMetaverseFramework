using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class AsyncCounter
    {
        public int count;
        int timer;
        public System.Action DOnComplete;
        public AsyncCounter(int count,System.Action DOnComplete) {
            this.count = count;
            this.DOnComplete = DOnComplete;
        }

        public void Tick() {
            timer++;
            if (timer>=count) {
                DOnComplete?.Invoke();
            }
        }

        public void Kill() {
            DOnComplete = null;
        }
    }
}