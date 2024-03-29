﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanguardServer.Concurrent;

namespace VanguardServer.Timer
{
    /// <summary>
    /// 定时任务（计时器）管理类
    /// </summary>
    public class TimerManager
    {
        private static TimerManager instance = null;
        public static TimerManager Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                        instance = new TimerManager();
                    return instance;
                }                
            }
        }

        /// <summary>
        /// 实现定时器的主要功能就是这个Timer类
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// 这个字典存储：任务id和任务模型的映射
        /// </summary>
        private ConcurrentDictionary<int, TimerModel> idModelDict = new ConcurrentDictionary<int, TimerModel>();

        /// <summary>
        /// 要移除的任务ID列表
        /// </summary>
        private List<int> removeList = new List<int>();

        /// <summary>
        /// 用来表示ID
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        public TimerManager()
        {
            timer = new System.Timers.Timer(10);
            timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// 达到时间间隔时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (removeList) {
                TimerModel tmpModel = null;
                foreach (var id in removeList)
                {
                    idModelDict.TryRemove(id, out tmpModel);
                }
                removeList.Clear();                
            }

            foreach (var model in idModelDict.Values)
            {
                // t1: 10 + 2 = 12
                // t2: 11
                // t3: 13
                if (model.Time <= DateTime.Now.Ticks)
                    model.Run();
            }
        }

        /// <summary>
        /// 添加定时任务 指定触发的时间 2020年1月11日10:46:16
        /// </summary>
        public void AddTimerEvent(DateTime datetime, TimerDelegate timerDelegate)
        {
            long delayTime = datetime.Ticks - DateTime.Now.Ticks;
            if (delayTime <= 0)
                return;
            AddTimerEvent(delayTime, timerDelegate);
        }

        /// <summary>
        /// 添加定时任务 指定延迟的时间 40秒之后
        /// </summary>
        /// <param name="delayTime">毫秒</param>
        /// <param name="timerDelegate"></param>
        public void AddTimerEvent(long delayTime, TimerDelegate timerDelegate)
        {
            TimerModel model = new TimerModel(id.Add_Get(), DateTime.Now.Ticks + delayTime, timerDelegate);
            idModelDict.TryAdd(model.Id, model);
        }
    }
}
