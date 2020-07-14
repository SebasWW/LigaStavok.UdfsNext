using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.WebSocket
{
	public class WebSocketClient : IDisposable, IWebSocketClient
	{
		private ClientWebSocket clientWebSocket;

		private readonly WebSocketClientOptions options;
		private readonly ILogger<WebSocketClient> logger;

		public event EventHandler<TextMessageReceivedEventArgs> OnMessage;
		public event EventHandler<EventArgs> OnConnected;
		public event EventHandler<EventArgs> OnDisconnected;

		private CancellationTokenSource cancellationTokenSource;

		public WebSocketClient(
			ILogger<WebSocketClient> logger,
			IOptions<WebSocketClientOptions> options
		)
		{
			this.logger = logger;
			this.options = options.Value; ;
		}

		public void Start(CancellationToken cancellationToken)
		{
			cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			Task.Run(() => StartAsync(cancellationTokenSource.Token));
		}

		public bool IsConnected { get => clientWebSocket != null && clientWebSocket.State == WebSocketState.Open; }

		public Task SendAsync(string text, CancellationToken cancellationToken)
		{
			var bytes = Encoding.UTF8.GetBytes(text);
			var array = new ArraySegment<byte>(bytes);

			return (clientWebSocket ?? throw new WebSocketException("Websocket is not connected."))
				.SendAsync(array, WebSocketMessageType.Text, true, cancellationToken);
		}

		private async Task StartAsync(CancellationToken cancellationToken)
		{
			ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[options.BufferSize]);

			while (!cancellationToken.IsCancellationRequested)
			{
				using (clientWebSocket = new ClientWebSocket())
				{
					// Websocket setup
					if (options.UseDefaultCredentials)
						clientWebSocket.Options.UseDefaultCredentials = true;
					else
					{
						clientWebSocket.Options.UseDefaultCredentials = false;

						clientWebSocket.Options.UseDefaultCredentials = false;
						clientWebSocket.Options.Credentials = new NetworkCredential(options.UserName, options.Password);
					}
					clientWebSocket.Options.UseDefaultCredentials = false;
					clientWebSocket.Options.Credentials = new NetworkCredential(options.UserName, options.Password);

					try
					{
						logger.LogInformation($"Websocket initializeing ... {options.Uri}");

						await clientWebSocket.ConnectAsync(options.Uri, cancellationToken);

						logger.LogInformation($"Websocket is connected {options.Uri}");

						// Event
						try
						{
							OnConnected?.Invoke(this, new EventArgs());
						}
						catch (Exception ex)
						{
							logger.LogError(ex, "WebSocketClient: Subscriber's OnConnected event error.");
						}

						while (clientWebSocket.State == WebSocketState.Open && (!cancellationToken.IsCancellationRequested))
						{
							using (var ms = new MemoryStream())
							{
								WebSocketReceiveResult result = null;

								do
								{
									try
									{
										result = await clientWebSocket.ReceiveAsync(buffer, cancellationToken);

										if (result.MessageType == WebSocketMessageType.Close)
										{
											await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close response received", cancellationToken);
											break;
										}
										else
										{
											ms.Write(buffer.Array, buffer.Offset, result.Count);

											if (result.EndOfMessage)
											{
												if (result.MessageType == WebSocketMessageType.Text)
												{
													ms.Position = 0;

													using (var sr = new StreamReader(ms))
													{
														var args = new TextMessageReceivedEventArgs()
														{
															MessageText = sr.ReadToEnd()
														};

														try
														{
															OnMessage?.Invoke(this, args);
														}
														catch (Exception ex)
														{
															logger.LogInformation(ex, "WebSocketClient: Subscriber's OmMessage event error.");
														}
													}
												}
											}
										}
									}
									catch (OperationCanceledException)
									{
										// shutdowning...
									}
									catch (Exception ex)
									{
										logger.LogError(ex, "WebSocketClient: Receiving error.");
									}
								}
								while (result != null && !result.EndOfMessage);
							}
						}

						// Event
						try
						{
							OnDisconnected?.Invoke(this, new EventArgs());
						}
						catch (Exception ex)
						{
							logger.LogError(ex, "WebSocketClient: Subscriber's OnDisconnected event error.");
						}
					}

					catch (Exception ex)
					{
						logger.LogError(ex, "WebSocketClient: Server connection error.");
						await Task.Delay(options.FailureReconnectDelay);
					}
				}
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					cancellationTokenSource?.Cancel();

					if (clientWebSocket != null)
					{
						if (clientWebSocket.State == WebSocketState.Open)
							clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None).GetAwaiter().GetResult();

						clientWebSocket.Dispose();
					}
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~WebSocketClientService()
		// {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
