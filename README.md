# NetService
C#! - Java that can only run in docker/on windows. 
Heres the Java Code this is based on that is platform agnostic
```
public class WmicIngest {
	private static final List<String> LOG_NAMES = Arrays.asList("system", "application", "security");
	private static final String COMMAND = "powershell.exe  Get-EventLog -LogName @ | Export-Csv \"D:\\tmp\\@&.csv\" -NoTypeInformation -UseCulture";
	private ExecutorService executor = null;

	public WmicIngest() {
		this.executor = this.createExecutor();
	}

	private ThreadPoolTaskExecutor taskExecutor() {
		ThreadPoolTaskExecutor executor = new ThreadPoolTaskExecutor();
		executor.setCorePoolSize(20);
		executor.setQueueCapacity(20);
		executor.setThreadNamePrefix("CELK-");
		return executor;
	}

	private ExecutorService createExecutor() {
		return Executors.newFixedThreadPool(20, this.taskExecutor());
	}

	public void exportLogsToCsv() {
		for (String log : LOG_NAMES) {
			String command = COMMAND.replaceAll("@", log).replace("&", "-" + Instant.now().getMillis() + "");
			ExportThread export = new ExportThread(command, this);
			this.executor.execute(export);
		}
	}

	private boolean executeExport(String command) throws Exception {
		Process powerShellProcess = Runtime.getRuntime().exec(command);

		String line;
		log.info("Running export logs command {}", command);
		BufferedReader stdout = new BufferedReader(new InputStreamReader(powerShellProcess.getInputStream()));
		while ((line = stdout.readLine()) != null) {
			log.info("STDOUT:: " + line);
		}
		stdout.close();
		BufferedReader stderr = new BufferedReader(new InputStreamReader(powerShellProcess.getErrorStream()));
		boolean isError = false;
		while ((line = stderr.readLine()) != null) {
			log.error("STDERR:: " + line);
			isError = true;
		}
		stderr.close();
		log.info("Export logs complete");
		powerShellProcess.getInputStream().close();
		powerShellProcess.getOutputStream().close();
		powerShellProcess.getErrorStream().close();
		powerShellProcess.destroyForcibly();
		powerShellProcess.destroy();

		return !isError;
	}

	static class ExportThread implements Runnable {
		private final String command;
		private final WmicIngest ingest;

		public ExportThread(String command, WmicIngest ingest) {
			this.command = command;
			this.ingest = ingest;
		}

		@Override
		public void run() {
			try {
				log.info("Executing command " + this.command);
				this.ingest.executeExport(this.command);
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

	public static void main(String[] args) throws Exception {
		WmicIngest wmic = new WmicIngest();
		wmic.exportLogsToCsv();
	}
}
```

