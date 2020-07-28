# SpringCard PC/SC + VAS SDK

## About this SDK

This PC/SC + VAS SDK is an extension to the [SpringCard PC/SC SDK](https://github.com/springcard/springcard.pcsc.sdk).

This SDK provides the source code for **PassKitRdr** and **SmartTapRdr**, two sample applications that demonstrates how to read NFC passes from an iPhone or an Android device, running either the **Apple VAS** protocol (Apple Wallet) or the **Google VAS** protocol (Smart Tap for Google Pay), and using a **SpringCard PC/SC contactless (NFC) reader**.

**PassKitRdr** and **SmartTapRdr** are written and C# and target the .NET framework v4.6.2 or greater. They run on Mono provided that PC/SC reader has been correctly configured for the Linux or Mac.

Both application rely on an underlying software library (`SpringCard.AppleVas.dll` and `SpringCard.GoogleVas.dll`). <u>This SDK provides the source code of the applications only</u>. The libraries are closed sources projects and provided only as binaries.

## License policy

Both libraries come with a time-limited license - <u>you'll have to restart the application every 10 minute</u>s. Contact SpringCard sales team on [www.springcard.com/en/contact](https://www.springcard.com/en/contact) for all inquiries regarding the pricing and licensing policy.

## Documentations

### PassKitRdr (Apple VAS reader)

* Read the [Getting started guide](https://github.com/springcard/springcard.pcsc-vas.sdk/tree/master/projects/dotnet/PassKitRdr) in the sample application's directory
* The documentation of the library is available at [docs.springcard.com/apis/NET/AppleVAS](https://docs.springcard.com/apis/NET/AppleVAS/)

### SmartTapRdr (Google VAS reader)

* Read the [Getting started guide](https://github.com/springcard/springcard.pcsc-vas.sdk/tree/master/projects/dotnet/SmartTapRdr) in the sample application's directory
* The documentation of the library is available at [docs.springcard.com/apis/NET/GoogleVAS](https://docs.springcard.com/apis/NET/GoogleVAS/)

## Restrictions, trademarks and copyright disclaimer

### Apple VAS

The `SpringCard.AppleVas.dll` library has been certified for interoperability with Apple products (MFI program) only on **SpringCard Prox'N'Roll HSP PC/SC** contactless reader, having the Apple ECP (enhanced contactless polling) option enabled. Using this library with another reader or without the ECP option enabled is not supported.

Apple, Apple Wallet, Apple Pay, iPhone and iPad are trademarks of Apple Inc.

***Apple Inc does not endorse nor sponsor this project in any way.***

### Google VAS

The `SpringCard.GoogleVas.dll` library is currently being certified for interoperability with Google Pay. Do not use it until the certification is achieved.

Google, Android and Google Pay are trademarks of Google Inc.

***Google Inc does not endorse nor sponsor this project in any way.***

## Legal disclaimer

THE SDK IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

## How to contact us

For technical information, this form is available:

[www.springcard.com/en/support/contact](https://www.springcard.com/en/support/contact)

For any commercial request, please use this other form:

[www.springcard.com/en/contact](https://www.springcard.com/en/contact)