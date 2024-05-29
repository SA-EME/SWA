<h1 align=center>Syslog Windows Agent</h1>

<p align=center>
  <a href="https://github.com/SA-EME/GitBenchmark"><img src="https://img.shields.io/badge/SWA.Core-0.1.2-%23f7df1e?style=for-the-badge" alt="SWA"/></a>
  <img src="https://img.shields.io/badge/LANGUAGE-CSHARP-ad2828?style=for-the-badge" alt="CSHARP"/>
</p>

<p align=center>
<img style="width: 400px" src="https://github.com/SA-EME/.github/blob/main/assets/img/evian.png" />
</p>

SWA is a service which will send the Windows Event Log to a syslog server.

# How it work
When a new log is written to the Windows event log, the application checks whether the log is defined in the rules file. If so, the application sends the log to the syslog server configured in the configuration file.