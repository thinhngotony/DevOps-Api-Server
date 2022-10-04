using SuperSimpleTcp;
using System;
using System.Reflection;
using System.Text;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Services
{
    public static class TCPService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static SimpleTcpServer server;

        public static void Run()
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigFile.TcpHost))
                    return;

                logger.Info($"Start TCP server : {ConfigFile.TcpHost} ");
                server = new SimpleTcpServer(ConfigFile.TcpHost);
                
                // set events
                server.Events.ClientConnected += ClientConnected;
                server.Events.ClientDisconnected += ClientDisconnected;
                server.Events.DataReceived += DataReceived;

                // let's go!
                server.Start();

                // once a client has connected...
                server.Send("[ClientIp:Port]", "Smart Shelf TCP Started!");
                logger.Info($"Smart Shelf TCP Started!");
            }
            catch(Exception ex)
            {
                logger.Error($"Start TCP server error: {ConfigFile.TcpHost}. {ex.Message} ");
            }
            
        }
        public static void ClientConnected(object sender, ConnectionEventArgs e)
        {
           //Console.WriteLine($"[{e.IpPort}] client connected");
            logger.Info($"[{e.IpPort}] client connected");
        }

        public static void ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            //Console.WriteLine($"[{e.IpPort}] client disconnected: {e.Reason}");
            logger.Info($"[{e.IpPort}] client disconnected: {e.Reason}");
        }

        public static void DataReceived(object sender, DataReceivedEventArgs e)
        {
            //Console.WriteLine($"[{e.IpPort}]: {Encoding.UTF8.GetString(e.Data)}");
            logger.Info($"[{e.IpPort}]: {Encoding.UTF8.GetString(e.Data)}");

            Ultil.DataFromClient = Encoding.UTF8.GetString(e.Data);

            server.Send(e.IpPort, "ACTION_REGISTER_SHELF_SUCCESS");
        }
    }
}
