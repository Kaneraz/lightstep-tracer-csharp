﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace LightStep
{
    /// <summary>
    ///     Options for configuring the LightStep tracer.
    /// </summary>
    public class Options
    {
        /// <summary>
        ///     An identifier for the Tracer.
        /// </summary>
        public readonly ulong TracerGuid = new Random().NextUInt64();

        /// <summary>
        ///     Creates a new set of options for the LightStep tracer.
        /// </summary>
        /// <param name="token">Project API key.</param>
        /// <param name="satelliteOptions">Satellite endpoint configuration.</param>
        /// <exception cref="ArgumentNullException">An API key is required.</exception>
        public Options(string token, SatelliteOptions satelliteOptions)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

            Tags = InitializeDefaultTags();
            ReportPeriod = TimeSpan.FromMilliseconds(5000);
            ReportTimeout = TimeSpan.FromSeconds(30);
            AccessToken = token;
            Satellite = satelliteOptions;
            UseHttp2 = true;
        }

        /// <summary>
        ///     API key for a LightStep project.
        /// </summary>
        public string AccessToken { get; set; }
        
        /// <summary>
        ///     True if the satellite connection should use HTTP/2, false otherwise.
        /// </summary>
        public bool UseHttp2;

        /// <summary>
        ///     LightStep Satellite endpoint configuration.
        /// </summary>
        public SatelliteOptions Satellite { get; set; }

        /// <summary>
        ///     How often the reporter will send spans to a LightStep Satellite.
        /// </summary>
        public TimeSpan ReportPeriod { get; set; }

        /// <summary>
        ///     Timeout for sending spans to a LightStep Satellite.
        /// </summary>
        public TimeSpan ReportTimeout { get; set; }

        /// <summary>
        ///     Tags that should be applied to each span generated by this tracer.
        /// </summary>
        public IDictionary<string, object> Tags { get; set; }

        private IDictionary<string, object> InitializeDefaultTags()
        {
            var attributes = new Dictionary<string, object>
            {
                [LightStepConstants.TracerPlatformKey] = LightStepConstants.TracerPlatformValue,
                [LightStepConstants.TracerPlatformVersionKey] = GetPlatformVersion(),
                [LightStepConstants.TracerVersionKey] = "0.3",
                [LightStepConstants.ComponentNameKey] = GetComponentName(),
                [LightStepConstants.HostnameKey] = GetHostName(),
                [LightStepConstants.CommandLineKey] = GetCommandLine()
            };
            return attributes;
        }

        private static string GetComponentName()
        {
            var entryAssembly = "";
            try
            {
                entryAssembly = Assembly.GetEntryAssembly().GetName().Name;
            }
            catch (NullReferenceException)
            {
                // could not get assembly name, possibly because we're running a test
                entryAssembly = "unknown";
            }  
            return entryAssembly;
        }

        private static string GetPlatformVersion()
        {
            return Environment.Version.ToString();
        }


        private static string GetHostName()
        {
           return Environment.MachineName;
        }

        private static string GetCommandLine()
        {
            return Environment.CommandLine;
        }
    }
}