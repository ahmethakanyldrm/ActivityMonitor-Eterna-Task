const WebSocket = require('ws');
const fs = require('fs');
const path = require('path');

// WebSocket sunucusu oluştur
const wss = new WebSocket.Server({ port: 3001 });

// Log dosyası için klasör oluştur
const logDir = path.join(__dirname, 'logs');
if (!fs.existsSync(logDir)){
    fs.mkdirSync(logDir);
}

console.log('WebSocket sunucusu başlatıldı (port: 3001)');

wss.on('connection', (ws) => {
    console.log('Yeni bir istemci bağlandı');

    ws.on('message', (data) => {
        try {
            // Gelen veriyi JSON olarak ayrıştır
            const processes = JSON.parse(data);
            
            // Log dosyasına yaz
            const logFile = path.join(logDir, `process_log_${new Date().toISOString().split('T')[0]}.json`);
            const logEntry = {
                timestamp: new Date().toISOString(),
                processes: processes
            };

            fs.appendFileSync(logFile, JSON.stringify(logEntry) + '\n');
            
            console.log(`${processes.length} process kaydedildi`);
        } catch (error) {
            console.error('Veri işleme hatası:', error);
        }
    });

    ws.on('close', () => {
        console.log('İstemci bağlantısı kapandı');
    });
});