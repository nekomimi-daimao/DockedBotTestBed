# DockedBotTestBed

UnityでビルドしたDedicated ServerをDockerのUbuntuで起動するサンプルリポジトリです。  
以下の手順で実行できます。

1. [PUN2](https://www.photonengine.com/en-US/pun) をインポートする
2. Dedicated ServerのScripting Define Symbolsに`PHOTON_UNITY_NETWORKING`を追加
3. Tools → Build → HeadlessBot でUnityアプリをビルド
4. [docker_build.sh](https://github.com/nekomimi-daimao/DockedBotTestBed/blob/main/Docker/docker_build.sh) を実行してimageをビルド
5. コンテナを立ち上げ  
   `docker run docked-bot:b12c32f -name dog -matching animal -realtime 012345-67890-1234-5678-9012345`

詳しいことはZennの記事にあります。

https://zenn.dev/nekomimi_daimao/articles/bf0e1aa55a8fe8
