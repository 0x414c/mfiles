using System.Net.NetworkInformation;


namespace FSOps {
    static class Networking {
        public static string GetFQDN () {
            var domainName = IPGlobalProperties.GetIPGlobalProperties ().DomainName;
            var hostName = IPGlobalProperties.GetIPGlobalProperties ().HostName;

            if (!hostName.EndsWith (domainName)) {
                return hostName + "." + domainName;
            } else {
                return hostName;
            }                   
        }
    }
}
