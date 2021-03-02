# AntiAFK
Anti AFK and Rage Quit for Tekno MW3 Infected Servers.
This script will work with IW4M if you use this plugin [AntiAFK](https://github.com/diamante0018/IW4M-Event).
Otherwise, you can use the internal Tekno Ban database. That is highly discouraged. Please use IW4M.
To use the internal Tekno database, you need to change the source code slightly. It will be easy as I left the old code, so uncomment where it is required.

- sv_path_for_ban_dbs Is the string dvar name for the ban folder. It can be server-specific if you have more than one server.cfg
- sv_ban_by_ip Is the bool dvar name for banning players by IP. It is automatically set to "true" if not set by the server.cfg
- sv_ban_by_guid Is the bool dvar name for banning players by HWID. It is not recommended to modify this dvar
- sv_kickBanTime Is the time a player will be temporarily be banned. Max value is one week, default is 3 hours (not original values, they are modified by TeknoMW3S.dll)
