#### Unity3D QR Code Sharing System

Enable players to share the puzzles they create with friends or on social media without diving into the complexities of multiplayer modes or requiring identifiable accounts. Integrate this QR Code sharing system into your game, inspired by the QR Code sharing system of [Mekorama](https://www.mekorama.com/) (not affiliated).

[![Youtube Video Demo](https://i.imgur.com/qFzPfsF.png)](https://www.youtube.com/watch?v=HuAT11Yv6qg)

Above is a Video demo for the asset.

There is also an online version of the demo scene [here](https://devilintheeden.itch.io/qrcode-share-demo) if you want to try it out.

The Unity package version is in the **Releases** section. In the future, there will also be a free Asset Store version if you wish to manage your packages that way.

---


To use this asset, first drag the **Packages** folder containing [ZXing.net](https://github.com/micjahn/ZXing.Net) to the root **Assets** Folder. ZXing is the base library that allows this asset to handle QR Codes.

Next, you can run the demo scene in the **DemoScene** folder (target platform: WebGL / need TextMesh Pro). The asset itself is cross-platform; you'll just need to write your own upload and download file system for other platforms. Here are some excellent free assets to get you started (not affiliated).

- (Desktop) [Runtime File Browser](https://assetstore.unity.com/packages/tools/gui/runtime-file-browser-113006) by yasirkula
- (Mobile) [Native Gallery for Android & iOS](https://assetstore.unity.com/packages/tools/integration/native-gallery-for-android-ios-112630) by yasirkula

**Clarification**: The local version include with the asset doesn't allow outside copy & paste. This is a known issue for Unity3D webGL build. Look at this [thread](https://forum.unity.com/threads/copy-paste-in-textfields-in-webgl-builds.1291664/) for possible solutions.

**Demo Art Resource Source**:

1. Milk tea Logo: from [svgrepo](https://www.svgrepo.com/svg/530622/milk-tea) under CC0 license.
2. Flight Logo: from [svgrepo](https://www.svgrepo.com/svg/438544/flight-round) under GP license.
3. Background Image: I take the photo myself in San Diego, California.

