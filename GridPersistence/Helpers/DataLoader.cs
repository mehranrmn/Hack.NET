using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using GridPersistence.data;
public static class DataLoader
{
    public static ObservableCollection<Student> LoadStudents()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "students.json");

        if (!File.Exists(filePath))
            return new ObservableCollection<Student>();

        string jsonData = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<ObservableCollection<Student>>(jsonData)
               ?? new ObservableCollection<Student>();
    }
}
