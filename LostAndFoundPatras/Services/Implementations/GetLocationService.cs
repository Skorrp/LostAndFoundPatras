using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace LostAndFoundPatras.Services.Implementations
{
	public class GetLocationService
	{
		readonly bool stopping = false;			// for stop tracking location
		public GetLocationService()
		{
		}

		public async Task Run(CancellationToken token)
		{
			await Task.Run(async () => {
				while (!stopping)
				{
					token.ThrowIfCancellationRequested();
					try
					{
						// get user location every 2 seconds
						await Task.Delay(2000);

						var request = new GeolocationRequest(GeolocationAccuracy.High);
						var location = await Geolocation.GetLocationAsync(request);
						// send location through messaging center
						if (location != null)
						{
							var message = new LocationMessage
							{
								Latitude = location.Latitude,
								Longitude = location.Longitude
							};

							Device.BeginInvokeOnMainThread(() =>
							{
								MessagingCenter.Send(message, "Location");
							});
						}
					}
					catch (Exception ex)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							var errormessage = new LocationErrorMessage();
							MessagingCenter.Send(errormessage, "LocationError");
						});
					}
				}
				return;
			}, token);
		}
	}
}
