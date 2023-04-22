using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class Statistics
    {
        private readonly List<List<float>> _timeSeries = new();
        private readonly string _objectName;
        private readonly string _type;
        private readonly string _typeID;
        private readonly string _axis;
        

        public Statistics(string objectName, string type, string typeID, string axis)
        {
            _objectName = objectName;
            _type = type;
            _typeID = typeID;
            _axis = axis;
        }
        
        
        public void AddTimeStep(float time, float yValue)
        {
            _timeSeries.Add(new List<float>() {time, yValue});
        }
        
        
        public void WriteTimeSeriesToCsv() {
            var fileName = $"time_series_{_objectName}_{_type}_{_axis}.csv";
            using var streamWriter = new StreamWriter(string.Format("{0}/{1}", Application.dataPath, fileName));
            streamWriter.NewLine = null;
            streamWriter.AutoFlush = false;
            streamWriter.NewLine = null;
            streamWriter.AutoFlush = false;
            streamWriter.WriteLine($"t,{_typeID}(t)");
            foreach (var timeStep in _timeSeries) {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
        
    }
}