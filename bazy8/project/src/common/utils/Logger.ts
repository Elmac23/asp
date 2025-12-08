export class Logger {
  static log(message: string): void {
    console.log(`[LOG] ${new Date().toISOString()}: ${message}`);
  }

  static error(message: string, error?: Error): void {
    console.error(`[ERROR] ${new Date().toISOString()}: ${message}`, error);
  }
}
