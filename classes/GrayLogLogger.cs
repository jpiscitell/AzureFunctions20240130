using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Sinks.Graylog;
//using Serilog;
// using Serilog.Sinks.Graylog.Core.Transport;

 public class GrayLogLogger
 {
     public static void Log(string LogData, string ld, string Level, string GrayLogHostnameOrAddress, int GrayLogPort, string GreyLogStream)
     {
         //Log data to GrayLog
         //Controled by db record where the application name is set in the appsettings.json file
         if (ld == "Y")
         {
             try
             {
                 //var loggerconfig = new LoggerConfiguration();
//                     .WriteTo.Graylog(
//                         new GraylogSinkOptions
//                         {
//                             HostnameOrAddress = GrayLogHostnameOrAddress,
//                             Port = GrayLogPort,
//                             //TransportType = TransportType.Udp,
//                             //Facility = GreyLogStream,
//                             ShortMessageMaxLength = 8192
//                         });

                 //var log = loggerconfig.CreateLogger();
                 //The level is passed in to help organize the log into Information, Error and Warning entries
                //if (Level == "I") {
                     //log.Information(LogData);    
                 //} else if (Level == "E") {
                     //log.Error(LogData);    
                 //} else if (Level == "W") {
                     //log.Warning(LogData);
                 //}
                
                 var x = LogData.Length;
             } catch (Exception ex)
             {
                string exS = ex.ToString();
                string exSS = "There was a graylog error: " + exS;
                 //r.RecordError("Error from recording to GrayLog: " + ex.ToString());
             }
         }
     }
 }