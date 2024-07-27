using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using SMSMessage.Interface;
using SMSMessage.Service;

namespace SMSMessage
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    events.AddAndroid(android => android
                        .OnCreate((activity, bundle) => 
                        {
                            var receiver = new SmsSentReceiver();
                            var intentFilter = new Android.Content.IntentFilter(SmsSentReceiver.SmsDeliveredAction);
                            Android.App.Application.Context.RegisterReceiver(receiver, intentFilter);
                        }));
#endif
                });

            builder.Services.AddSingleton<IMessageService, MessageService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
