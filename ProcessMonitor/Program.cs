using System.Diagnostics;
using System.Text.Json;
using WebSocketSharp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Process Monitor başlatılıyor...");

        // Sonsuz döngü ile bağlantı ve veri gönderimi sürdürülür
        while (true)
        {
            try
            {
                // WebSocket bağlantısı oluşturulur
                using (var ws = new WebSocket("ws://localhost:3001"))
                {
                    // Bağlantı açıldığında tetiklenir
                    ws.OnOpen += (sender, e) =>
                        Console.WriteLine("Sunucuya bağlandı!");

                    // Hata durumunda tetiklenir
                    ws.OnError += (sender, e) =>
                        Console.WriteLine($"Hata: {e.Message}");

                    // Bağlantı kapandığında tetiklenir
                    ws.OnClose += (sender, e) =>
                        Console.WriteLine("Bağlantı kapandı. Yeniden bağlanmaya çalışılacak...");

                    // Sunucuya bağlan
                    ws.Connect();

                    // Bağlantı açık olduğu sürece veri gönder
                    while (ws.ReadyState == WebSocketState.Open)
                    {
                        try
                        {
                            // Çalışan işlemleri al ve JSON formatına dönüştür
                            var processes = GetRunningProcesses();
                            var jsonData = JsonSerializer.Serialize(processes);

                            // Veriyi WebSocket üzerinden gönder
                            ws.Send(jsonData);
                            Console.WriteLine($"Veri gönderildi: {processes.Count} process bulundu");

                            // 5 saniye bekle
                            await Task.Delay(5000);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Veri gönderme hatası: {ex.Message}");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Bağlantı hatalarında bekleme ve yeniden bağlanma
                Console.WriteLine($"Bağlantı hatası: {ex.Message}");
                Console.WriteLine("5 saniye sonra yeniden bağlanmaya çalışılacak...");
                await Task.Delay(5000);
            }
        }
    }

    // Çalışan işlemleri getirir
    static List<ProcessInfo> GetRunningProcesses()
    {
        var processes = Process.GetProcesses()
            .Select(p => new ProcessInfo
            {
                ProcessName = p.ProcessName,
                Id = p.Id,
                StartTime = GetProcessStartTime(p),
                MemoryUsage = p.WorkingSet64 / 1024 / 1024 // MB cinsinden bellek kullanımı
            })
            .ToList();

        return processes;
    }

    // İşlem başlangıç zamanını güvenli bir şekilde alır
    static DateTime? GetProcessStartTime(Process process)
    {
        try
        {
            return process.StartTime;
        }
        catch
        {
            return null;
        }
    }
}

// İşlem bilgilerini tutan sınıf
class ProcessInfo
{
    public string ProcessName { get; set; }
    public int Id { get; set; }
    public DateTime? StartTime { get; set; }
    public long MemoryUsage { get; set; }
}