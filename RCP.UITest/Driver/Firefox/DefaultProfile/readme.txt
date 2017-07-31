prefs.js
- set "browser.download.folderList" = 2
	- this forces FireFox to use the "browser.download.dir" pref which is set in BaseFirefoxProfile.cs
- proxy settings
	- "network.proxy.backup.ftp" = "firewall..com"
	- "network.proxy.backup.ftp_port" = 8080
	- "network.proxy.backup.socks" = "firewall..com"
	- "network.proxy.backup.socks_port" = 8080
	- "network.proxy.backup.ssl" = "firewall..com"
	- "network.proxy.backup.ssl_port" = 8080
	- "network.proxy.ftp" = "firewall..com"
	- "network.proxy.ftp_port" = 8080
	- "network.proxy.http" = "firewall..com"
	- "network.proxy.http_port" = 8080
	- "network.proxy.no_proxies_on" = "localhost, 127.0.0.1, *.com, *.com, *ftp*..com, , ,<local>"
		- if you add new servers/domain names then this pref will need to be updated
	- "network.proxy.share_proxy_settings" = true
	- "network.proxy.socks" = "firewall..com"
	- "network.proxy.socks_port" = 8080
	- "network.proxy.ssl" = "firewall..com"
	- "network.proxy.ssl_port" = 8080
	- "network.proxy.type" = 1

mimeTypes.rdf
- set NC:alwaysAsk="false" for the "vnd.openxmlformats-officedocument.spreadsheetml.sheet" mimetype
	- this prevents FireFox from ever showing a save as dialog when downloading an xlsx file
	- if more file types should be saved without asking, then this file will need to be updated