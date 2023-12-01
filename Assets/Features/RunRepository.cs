using System;
using UnityEngine;

namespace Features
{
    public static class RunRepository
    {
        private const string RunIDKey = "Run_ID";
        private const string RunIDsKey = "Runs_ID";

        public static void CreateNew()
        {
            var runKey = Guid.NewGuid().ToString();
            PlayerPrefs.SetString(RunIDKey, runKey);
            PlayerPrefs.SetString(RunIDsKey, string.Join(",", GetAllRuns(), runKey));
            PlayerPrefs.Save();
        }

        private static string GetAllRuns()
        {
            return PlayerPrefs.GetString(RunIDsKey, "");
        }

        public static string GetCurrent()
        {
            return PlayerPrefs.GetString(RunIDKey, Guid.NewGuid().ToString());
        }

        public static void LoadRun(int slot)
        {
            var runs = GetAllRuns().Split(",",StringSplitOptions.RemoveEmptyEntries);
            PlayerPrefs.SetString(RunIDKey, runs[slot]);
            PlayerPrefs.Save();

        }
    }
}