using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoEasy.Common
{
    /// <summary>
    /// 定时执行任务的辅助类
    /// </summary>
    public class TaskHelper
    {
        #region 任务集合
        static Dictionary<Time, Action> dicEveryDay = new Dictionary<Time, Action>();
        static Dictionary<Time, MethodAndArgs> dicEveryDayPara = new Dictionary<Time, MethodAndArgs>();

        static Dictionary<WeekAndTime, Action> dicEveryWeek = new Dictionary<WeekAndTime, Action>();
        static Dictionary<WeekAndTime, MethodAndArgs> dicEveryWeekPara = new Dictionary<WeekAndTime, MethodAndArgs>();

        static Dictionary<DayInMonthAndTime, Action> dicEveryMonth = new Dictionary<DayInMonthAndTime, Action>();
        static Dictionary<DayInMonthAndTime, MethodAndArgs> dicEveryMonthPara = new Dictionary<DayInMonthAndTime, MethodAndArgs>();

        static Dictionary<DayInYearAndTime, Action> dicEveryYear = new Dictionary<DayInYearAndTime, Action>();
        static Dictionary<DayInYearAndTime, MethodAndArgs> dicEveryYearPara = new Dictionary<DayInYearAndTime, MethodAndArgs>();

        static Dictionary<OnceTime, Action> dicOnce = new Dictionary<OnceTime, Action>();
        static Dictionary<OnceTime, MethodAndArgs> dicOncePara = new Dictionary<OnceTime, MethodAndArgs>();

        #endregion

        #region 添加任务
        #region 无参任务
        /// <summary>
        /// 添加 在每天的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        /// <param name="action">无参数无返回值的方法</param>
        static public void AddTask(TimeSpan execTime, Action action)
        {
            dicEveryDay.Add(new Time { ExecTime = execTime }, action);
        }

        /// <summary>
        /// 添加 在每周几的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        /// <param name="weekN">星期几</param>
        /// <param name="action">无参数无返回值的方法</param>
        static public void AddTask(DayOfWeek weekN, TimeSpan execTime, Action action)
        {
            dicEveryWeek.Add(new WeekAndTime { WeekN = weekN, ExecTime = execTime }, action);
        }

        /// <summary>
        /// 添加 在每月几号的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        ///<param name="day">每月的几号,传1到31之间的数字</param>
        /// <param name="action">无参数无返回值的方法</param>
        static public void AddTask(int day, TimeSpan execTime, Action action)
        {
            dicEveryMonth.Add(new DayInMonthAndTime { Day = day, ExecTime = execTime }, action);
        }

        /// <summary>
        /// 添加 在每年的几月几号的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        ///<param name="day">几号,传1到31之间的数字</param>
        ///<param name="month">几月,传1到12之间的数字</param>
        /// <param name="action">无参数无返回值的方法</param>
        static public void AddTask(int month, int day, TimeSpan execTime, Action action)
        {
            dicEveryYear.Add(new DayInYearAndTime { Month = month, Day = day, ExecTime = execTime }, action);
        }
        #endregion

        #region 带参数任务
        /// <summary>
        /// 添加 在每天的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        /// <param name="action">有一个boject参数且无返回值的方法</param>
        /// <param name="argsExpress">用于返回action方法所需参数的演绎法中表达式</param>
        static public void AddTask(TimeSpan execTime, Action<object> action, ArgsExpress argsExpress)
        {
            dicEveryDayPara.Add(new Time { ExecTime = execTime }, new MethodAndArgs { Method = action, Args = argsExpress });
        }

        /// <summary>
        /// 添加 在每周几的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        /// <param name="weekN">星期几</param>
        /// <param name="action">有一个boject参数且无返回值的方法</param>
        /// <param name="argsExpress">用于返回action方法所需参数的演绎法中表达式</param>
        static public void AddTask(DayOfWeek weekN, TimeSpan execTime, Action<object> action, ArgsExpress argsExpress)
        {
            dicEveryWeekPara.Add(new WeekAndTime { WeekN = weekN, ExecTime = execTime }, new MethodAndArgs { Method = action, Args = argsExpress });
        }

        /// <summary>
        /// 添加 在每月几号的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        ///<param name="day">每月的几号,传1到31之间的数字</param>
        /// <param name="action">有一个boject参数且无返回值的方法</param>
        /// <param name="argsExpress">用于返回action方法所需参数的演绎法中表达式</param>
        static public void AddTask(int day, TimeSpan execTime, Action<object> action, ArgsExpress argsExpress)
        {
            dicEveryMonthPara.Add(new DayInMonthAndTime { Day = day, ExecTime = execTime }, new MethodAndArgs { Method = action, Args = argsExpress });
        }

        /// <summary>
        /// 添加 在每年的几月几号的什么时间执行的任务
        /// </summary>
        /// <param name="execTime">执行时间,可以传TimeSpan.Parse("02:12")这样的参数</param>
        ///<param name="day">几号,传1到31之间的数字</param>
        ///<param name="month">几月,传1到12之间的数字</param>
        /// <param name="action">有一个boject参数且无返回值的方法</param>
        /// <param name="argsExpress">用于返回action方法所需参数的演绎法中表达式</param>
        static public void AddTask(int month, int day, TimeSpan execTime, Action<object> action, ArgsExpress argsExpress)
        {
            dicEveryYearPara.Add(new DayInYearAndTime { Month = month, Day = day, ExecTime = execTime }, new MethodAndArgs { Method = action, Args = argsExpress });
        }
        #endregion

        #region 只执行一次的任务

        /// <summary>
        /// 添加在指定时间执行一次的任务
        /// </summary>
        /// <param name="execTime">执行时间,必须大于当前时间,否则将不会执行</param>
        /// <param name="action">无参数且无返回值的方法</param>
        static public void AddTask(DateTime execTime, Action action)
        {
            dicOnce.Add(new OnceTime { ExecTime = execTime }, action);
        }

        /// <summary>
        /// 添加在指定时间执行一次的任务
        /// </summary>
        /// <param name="execTime">执行时间,必须大于当前时间,否则将不会执行</param>
        /// <param name="action">有一个boject参数且无返回值的方法</param>
        /// <param name="argsExpress">用于返回action方法所需参数的演绎法中表达式</param>
        static public void AddTask(DateTime execTime, Action<object> action, ArgsExpress argsExpress)
        {
            dicOncePara.Add(new OnceTime { ExecTime = execTime }, new MethodAndArgs { Args = argsExpress, Method = action });
        }


        #endregion
        #endregion

        /// <summary>
        /// 启动任务,让其自动在指定的时间运行任务,当任务添加完毕后需要调用此方法
        /// </summary>
        static public void Start()
        {
            int count = 0;
            foreach (var item in dicOnce)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecOnce(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicOncePara)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecOnce(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryDay)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryDayTask(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryDayPara)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryDayTask(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryWeek)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryWeekTask(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryWeekPara)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryWeekTask(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryMonth)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryMonthTask(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryMonthPara)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryMonthTask(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryYear)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryYearTask(item.Key, item.Value)));
                ++count;
            }
            foreach (var item in dicEveryYearPara)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => ExecEveryYearTask(item.Key, item.Value)));
                ++count;
            }
            Utility.Logger.Info("任务自动执行引擎已启动,共有" + count + "个任务待执行");
        }

        #region 任务执行方法
        /// <summary>
        /// 每天执行的方法
        /// </summary>
        static void ExecEveryDayTask(Time time, Action action)
        {
            ExceptionHelper.ExceptionRecord(() =>
            {
                string methodFullName = action.Method.DeclaringType.ToString() + " " + action.Method;
                while (true)
                {
                    TimeSpan sleepTime = CalcTime(time.ExecTime);
                    Thread.Sleep(sleepTime);

                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                    action.Invoke();
                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                    Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
                }
            }, null, false);
        }

        /// <summary>
        /// 每天执行的方法--带参数
        /// </summary>
        static void ExecEveryDayTask(Time time, MethodAndArgs action)
        {
            ExceptionHelper.ExceptionRecord(() =>
            {
                string methodFullName = action.Method.Method.DeclaringType.ToString() + " " + action.Method.Method;
                while (true)
                {
                    TimeSpan sleepTime = CalcTime(time.ExecTime);
                    Thread.Sleep(sleepTime);

                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                    action.Method.Invoke(action.Args.Invoke());
                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                    Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
                }
            }, null, false);
        }

        /// <summary>
        /// 每周执行的方法
        /// </summary>
        static void ExecEveryWeekTask(WeekAndTime weekTime, Action action)
        {
            ExceptionHelper.ExceptionRecord(() =>
            {
                string methodFullName = action.Method.DeclaringType.ToString() + " " + action.Method;
                while (true)
                {
                    TimeSpan sleepTime = CalcWeekAndTime(weekTime.WeekN, weekTime.ExecTime);
                    Thread.Sleep(sleepTime);

                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                    action.Invoke();
                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                    Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
                }
            }, null, false);
        }

        /// <summary>
        /// 每周执行的方法--带参数
        /// </summary>
        static void ExecEveryWeekTask(WeekAndTime weekTime, MethodAndArgs action)
        {
            ExceptionHelper.ExceptionRecord(() =>
            {
                string methodFullName = action.Method.Method.DeclaringType.ToString() + " " + action.Method.Method;
                while (true)
                {
                    TimeSpan sleepTime = CalcWeekAndTime(weekTime.WeekN, weekTime.ExecTime);
                    Thread.Sleep(sleepTime);

                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                    action.Method.Invoke(action.Args.Invoke());
                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                    Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
                }
            }, null, false);
        }


        /// <summary>
        /// 每月执行的方法
        /// </summary>
        static void ExecEveryMonthTask(DayInMonthAndTime monthTime, Action action)
        {
            ExceptionHelper.ExceptionRecord(() =>
            {
                string methodFullName = action.Method.DeclaringType.ToString() + " " + action.Method;
                while (true)
                {
                    TimeSpan sleepTime = CalcMonthAndTime(monthTime.Day, monthTime.ExecTime);
                    LongSleep(sleepTime);

                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                    action.Invoke();
                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                    Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
                }
            }, null, false);

        }

        /// <summary>
        /// 每月执行的方法--带参数
        /// </summary>
        static void ExecEveryMonthTask(DayInMonthAndTime monthTime, MethodAndArgs action)
        {
            ExceptionHelper.ExceptionRecord(() =>
           {
               string methodFullName = action.Method.Method.DeclaringType.ToString() + " " + action.Method.Method;
               while (true)
               {
                   TimeSpan sleepTime = CalcMonthAndTime(monthTime.Day, monthTime.ExecTime);
                   LongSleep(sleepTime);

                   Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                   action.Method.Invoke(action.Args.Invoke());
                   Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                   Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
               }
           }, null, false);
        }


        /// <summary>
        /// 每年执行的方法
        /// </summary>
        static void ExecEveryYearTask(DayInYearAndTime yearTime, Action action)
        {
            ExceptionHelper.ExceptionRecord(() =>
            {
                string methodFullName = action.Method.DeclaringType.ToString() + " " + action.Method;
                while (true)
                {
                    TimeSpan sleepTime = CalcYearAndTime(yearTime.Month, yearTime.Day, yearTime.ExecTime);
                    LongSleep(sleepTime);

                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                    action.Invoke();
                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                    Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
                }
            }, null, false);
        }

        /// <summary>
        /// 每年执行的方法--带参数
        /// </summary>
        static void ExecEveryYearTask(DayInYearAndTime yearTime, MethodAndArgs action)
        {
            ExceptionHelper.ExceptionRecord(() =>
           {
               string methodFullName = action.Method.Method.DeclaringType.ToString() + " " + action.Method.Method;
               while (true)
               {
                   TimeSpan sleepTime = CalcYearAndTime(yearTime.Month, yearTime.Day, yearTime.ExecTime);
                   LongSleep(sleepTime);

                   Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                   action.Method.Invoke(action.Args.Invoke());
                   Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                   Thread.Sleep(1000);//休眠1秒,防止方法在1秒内运行完后多次运行
               }
           }, null, false);
        }

        /// <summary>
        /// 只在指定时间执行一次的方法
        /// </summary>
        static void ExecOnce(OnceTime onceTime, Action action)
        {
            ExceptionHelper.ExceptionRecord(() =>
            {
                string methodFullName = action.Method.DeclaringType.ToString() + " " + action.Method;
                TimeSpan sleepTime = CalcFixedTime(onceTime.ExecTime);
                if (sleepTime.Ticks >= 0)
                {
                    LongSleep(sleepTime);

                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                    action.Invoke();
                    Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");

                }
            }, null, false);
        }

        /// <summary>
        /// 只在指定时间执行一次的方法
        /// </summary>
        static void ExecOnce(OnceTime onceTime, MethodAndArgs action)
        {
            ExceptionHelper.ExceptionRecord(() =>
          {
              string methodFullName = action.Method.Method.DeclaringType.ToString() + " " + action.Method.Method;
              TimeSpan sleepTime = CalcFixedTime(onceTime.ExecTime);
              if (sleepTime.Ticks >= 0)
              {
                  LongSleep(sleepTime);

                  Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行");
                  action.Method.Invoke(action.Args.Invoke());
                  Utility.Logger.Info("任务" + methodFullName + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行完毕");
              }
          }, null, false);
        }

        #endregion

        #region 辅助类与辅助方法
        #region 参数封装
        /// <summary>
        /// 参数委托,用于传递动态参数表达式,如DateTime.Now之类的,每次执行时参数的值都不一样,不能直接写一个固定的值
        /// </summary>
        /// <returns></returns>
        public delegate object ArgsExpress();

        class MethodAndArgs
        {
            /// <summary>
            /// 要执行的方法
            /// </summary>
            public Action<object> Method { get; set; }
            /// <summary>
            /// 产生参数的表达式或方法
            /// </summary>
            public ArgsExpress Args { get; set; }
        }

        /// <summary>
        ///参数封装是为了可以添加同一时间执行多个任务,否则会因为添加时间相的任务时,导致重复的键异常
        /// </summary>
        class Time
        {
            public TimeSpan ExecTime { get; set; }
        }
        /// <summary>
        ///参数封装是为了可以添加同一时间执行多个任务,否则会因为添加时间相的任务时,导致重复的键异常
        /// </summary>
        class OnceTime
        {
            public DateTime ExecTime { get; set; }
        }
        class WeekAndTime
        {
            public DayOfWeek WeekN { get; set; }
            public TimeSpan ExecTime { get; set; }
        }

        class DayInMonthAndTime
        {
            public int Day { get; set; }
            public TimeSpan ExecTime { get; set; }
        }

        class DayInYearAndTime
        {
            public int Month { get; set; }
            public int Day { get; set; }
            public TimeSpan ExecTime { get; set; }
        }
        #endregion

        /// <summary>
        /// 长休眠,普通休眠最多只能休眠24天
        /// </summary>
        /// <param name="sleepTime"></param>
        static void LongSleep(TimeSpan sleepTime)
        {
            if (sleepTime.TotalMilliseconds > int.MaxValue)//数值太大,不能直接休眠,会产生数据范围异常
            {
                int sleepCount = (int)(sleepTime.TotalMilliseconds / int.MaxValue);
                Thread.Sleep((int)(sleepTime.TotalMilliseconds % int.MaxValue));
                for (int i = 0; i < sleepCount; i++)
                {
                    Thread.Sleep(int.MaxValue);
                }
            }
            else
            {
                Thread.Sleep(sleepTime);
            }
        }

        #region 时间计算


        /// <summary>
        /// 计算从现在到指定时间还要多久
        /// </summary>
        /// <param name="datetime">指定时间</param>
        /// <returns></returns>
        static TimeSpan CalcFixedTime(DateTime datetime)
        {
            return datetime.Subtract(DateTime.Now);
        }

        /// <summary>
        /// 计算还有多久到指定的时间
        /// </summary>
        /// <param name="time">指定的时间 格式22:30:26</param>
        /// <returns></returns>
        static TimeSpan CalcTime(TimeSpan time)
        {
            TimeSpan timeMax = DateTime.Parse("23:59:59").AddMilliseconds(999).TimeOfDay;
            TimeSpan timeCur = DateTime.Parse(DateTime.Now.ToString("HH:mm:ss")).TimeOfDay;//去毫秒,直接DateTime.Now.TimeOfDay是带毫秒的,会导致最后执行时间4舍5入后提前1秒
            TimeSpan tmp = timeCur.Subtract(time);

            TimeSpan sleepTime = TimeSpan.Zero;
            if (tmp > TimeSpan.Zero)
            {
                sleepTime = timeMax.Subtract(tmp.Duration());
            }
            else
            {
                sleepTime = tmp.Duration();
            }
            return sleepTime;

        }

        /// <summary>
        /// 计算还有多久到最近一个星期几的某个时间
        /// </summary>
        /// <param name="weekN">星期几</param>
        /// <param name="time">指定的时间 格式22:30:26</param>
        /// <returns></returns>
        static TimeSpan CalcWeekAndTime(DayOfWeek weekN, TimeSpan time)
        {
            int nextDay = 0;

            DateTime now = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                if (now.AddDays(i).DayOfWeek == weekN)
                {
                    nextDay = i;
                    break;
                }
            }

            if (now.TimeOfDay > time)//如果当前的时间已经大于指定时间,那么返回下个周的
            {
                nextDay = nextDay > 0 ? nextDay - 1 : 6;//日期减去1是因为后边还要加上时间的部分
            }
            TimeSpan toTime = CalcTime(time);
            return new TimeSpan(nextDay, toTime.Hours, toTime.Minutes, toTime.Seconds);
        }

        /// <summary>
        /// 计算还有多久到最近一个月几号的某个时间
        /// </summary>
        /// <param name="monthDay">几号,1到31之间的数字</param>
        /// <param name="time">指定的时间 格式22:30:26</param>
        /// <returns></returns>
        static TimeSpan CalcMonthAndTime(int monthDay, TimeSpan time)
        {
            int nextDay = 0;
            int maxDay = 0;
            DateTime now = DateTime.Now;
            bool passed = false;//是否已经过了指定的日期与时间
            if (now.Day > monthDay || (now.Day == monthDay && now.TimeOfDay > time))//时间已过
            {
                DateTime nextMonth = now.AddMonths(1);
                maxDay = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                passed = true;
            }
            else
            {
                maxDay = DateTime.DaysInMonth(now.Year, now.Month);
            }
            monthDay = monthDay > maxDay ? maxDay : monthDay;//不能超过每月最大的一天

            if (passed)
            {
                nextDay = DateTime.DaysInMonth(now.Year, now.Month) - now.Day + monthDay;
            }
            else
            {
                nextDay = monthDay - now.Day;
            }

            if (now.TimeOfDay > time)
            {
                nextDay = nextDay > 0 ? nextDay - 1 : DateTime.DaysInMonth(now.Year, now.Month) - 1;////日期减去1是因为后边还要加上时间的部分
            }
            TimeSpan toTime = CalcTime(time);

            return new TimeSpan(nextDay, toTime.Hours, toTime.Minutes, toTime.Seconds);
        }

        /// <summary>
        /// 计算还有多久到最近一年几月几号的某个时间
        /// </summary>
        /// <param name="month">月份,1到12之间的数字</param>
        /// <param name="day">月份中的天数,1到31之间的数字</param>
        /// <param name="time">指定的时间 格式22:30:26</param>
        /// <returns></returns>
        static TimeSpan CalcYearAndTime(int month, int day, TimeSpan time)
        {
            int nextDay = 0;
            int maxDay = 0;
            DateTime now = DateTime.Now;

            bool passed = false;//时间是否已过
            if (now.Month > month || (now.Month == month && now.Day > day) || (now.Month == month && now.Day == day && now.TimeOfDay > time))
            {
                passed = true;
                DateTime nextYear = now.AddYears(1);
                maxDay = DateTime.DaysInMonth(nextYear.Year, month);
            }
            else
            {
                maxDay = DateTime.DaysInMonth(now.Year, month);
            }
            day = day > maxDay ? maxDay : day;

            if (passed)
            {
                nextDay = DateTime.IsLeapYear(now.Year) ? 366 - now.DayOfYear : 365 - now.DayOfYear;
                nextDay = nextDay + DateTime.Parse(now.AddYears(1).Year + "-" + month + "-" + day).DayOfYear;
            }
            else
            {
                nextDay = DateTime.Parse(now.Year + "-" + month + "-" + day).Subtract(now.Date).Days;
            }

            if (now.TimeOfDay > time)
            {
                nextDay = nextDay > 0 ? nextDay - 1 : (DateTime.IsLeapYear(now.Year) ? 366 : 365) - 1;////日期减去1是因为后边还要加上时间的部分
            }

            TimeSpan toTime = CalcTime(time);
            return new TimeSpan(nextDay, toTime.Hours, toTime.Minutes, toTime.Seconds);

        }

        #endregion
        #endregion
    }

}
