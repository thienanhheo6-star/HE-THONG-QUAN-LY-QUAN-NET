using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace QLQuanNet
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
    }

    public class Shift
    {
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal HourlyRate { get; set; }
    }

    public class EmployeeService
    {
        private List<Employee> employees = new List<Employee>();
        private List<Shift> shifts = new List<Shift>();
        private readonly string dataDir;
        private readonly string employeesFile;

        public EmployeeService()
        {
            dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "QLQuanNet_Data");
            employeesFile = Path.Combine(dataDir, "employees.xml");
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            try
            {
                if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
                if (!File.Exists(employeesFile))
                {
                    employees = new List<Employee>
                    {
                        new Employee { EmployeeId = 1, Name = "Admin", Phone = "", Salary = 0, Position = "Administrator", HireDate = DateTime.Now }
                    };
                    SaveToFile();
                    return;
                }
                var xs = new XmlSerializer(typeof(List<Employee>));
                using (var fs = File.OpenRead(employeesFile))
                {
                    employees = (List<Employee>)xs.Deserialize(fs);
                }
            }
            catch
            {
                employees = new List<Employee>();
            }
        }

        private void SaveToFile()
        {
            try
            {
                var xs = new XmlSerializer(typeof(List<Employee>));
                using (var fs = File.Create(employeesFile))
                {
                    xs.Serialize(fs, employees);
                }
            }
            catch { }
        }
    }
}
