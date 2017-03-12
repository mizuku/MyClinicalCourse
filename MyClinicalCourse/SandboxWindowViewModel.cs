namespace MyClinicalCourse
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    /// <summary>
    /// 無理やりデータこねくり回す君
    /// </summary>
    public class SandboxWindowViewModel : NotificationBase
    {
        private List<CourseViewModel> dayCourses;
        private List<CourseViewModel> weekCourses;
        static private Random rnd = new Random();

        private ObservableCollection<CourseViewModel> _courses;
        public ObservableCollection<CourseViewModel> Courses
        {
            get => this._courses;
            set => SetProperty(ref this._courses, value);
        }

        public ObservableCollection<(string, string)> Tables;

        public ObservableCollection<string> Periods { get; set; } = new ObservableCollection<string>() { "日", "週" };
        private string _selectedPeriod = "日";
        public string SelectedPeriod
        {
            get => this._selectedPeriod;
            set
            {
                if (SetProperty(ref _selectedPeriod, value))
                {
                    this.PeriodsSelectionChanged(value);
                }
            }
        }

        public SandboxWindowViewModel() : base()
        {
            this.dayCourses = Enumerable.Range(0, 4)
                .Select(i => new CourseViewModel()
                {
                    Label = $"2017/03/1{i}",
                    BloodPressure = 90.0d + (i % 2 == 0 ? 3d * i : -3d * i),
                    BodyTemperature = 36.0d + (i % 2 == 0 ? 0.2d * i : -0.2d * i),
                    Pulse = 100.0d + (i % 2 == 0 ? -2d * i : 2d * i),
                    Breathing = 20.0d + (i % 2 == 0 ? -1d * i : i)
                })
                .ToList();
            foreach (var c in this.dayCourses)
            {
                c.TableItems = new OrderedDictionary()
                {
                    { "血圧", c.BloodPressure.ToString() },
                    { "体温", c.BodyTemperature.ToString() },
                    { "脈拍", c.Pulse.ToString() },
                    { "呼吸数", c.Breathing.ToString() },
                    { "可変１", rnd.Next().ToString() },
                    { "可変２", rnd.Next().ToString() },
                    { "可変３", rnd.Next().ToString() },
                };
            }

            this.weekCourses = Enumerable.Range(0, 4)
                .Select(i => new CourseViewModel()
                {
                    Label = $"2017/03 第{i + 1}週",
                    BloodPressure = 90.0d + (i % 2 == 0 ? 5d * i : -2d * i),
                    BodyTemperature = 36.0d + (i % 2 == 0 ? 0.1d * i : -0.2d * i),
                    Pulse = 100.0d + (i % 2 == 0 ? -4d * i : 4d * i),
                    Breathing = 20.0d + (i % 2 == 0 ? -2d * i : 3d * i)
                })
                .ToList();
            foreach (var c in this.weekCourses)
            {
                c.TableItems = new OrderedDictionary()
                {
                    { "血圧", c.BloodPressure.ToString() },
                    { "体温", c.BodyTemperature.ToString() },
                    { "脈拍", c.Pulse.ToString() },
                    { "呼吸数", c.Breathing.ToString() },
                    { "可変１", rnd.Next().ToString() },
                    { "可変２", rnd.Next().ToString() },
                    { "可変３", rnd.Next().ToString() },
                };
            }

            this.SetCurrentModels(true);
        }

        private void PeriodsSelectionChanged(string newValue)
        {
            this.SetCurrentModels(0 == this.Periods.IndexOf(newValue));
        }

        public void SetCurrentModels(bool isDay)
        {
            if (isDay)
            {
                this.Courses = new ObservableCollection<CourseViewModel>(this.dayCourses);
            }
            else
            {
                this.Courses = new ObservableCollection<CourseViewModel>(this.weekCourses);
            }
        }

        public void AddCourseItem()
        {
            this.Courses.Add(new CourseViewModel()
            {
                Label = "追加アイテム",
                BloodPressure = 90.0d,
                BodyTemperature = 36.0d,
                Pulse = 100.0d,
                Breathing = 20.0d,
                TableItems = new OrderedDictionary()
                {
                    { "血圧", "90" },
                    { "体温", "36" },
                    { "脈拍", "100" },
                    { "呼吸数", "20" },
                    { "可変１", rnd.Next().ToString() },
                    { "可変２", rnd.Next().ToString() },
                    { "可変３", rnd.Next().ToString() },
                }
            });
        }
    }
}
