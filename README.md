# C# で学ぶアーキテクチャパターン

在庫管理アプリを題材に、C# でよく使われるアーキテクチャパターンを学ぶ教材です。
各章では、パターンを使わない **Bad コード** から始めて「何に困るのか」「なぜこの仕組みにしたのか」を確認し、そのあとでサンプルコードを読み解きます。

- 対象読者: C# の基本文法（クラス、インターフェース、ラムダ式、async/await）が分かる人
- 環境: .NET 10（LTS）SDK
- 第1部の WinForms / WPF のサンプルは Windows でのみ動きます

## 目次

### 第1部 UI層のパターン

画面とロジックをどう分けるか。

| 章 | 内容 | サンプル |
|---|---|---|
| [第0章 導入：なぜ分けるのか](docs/part1/00-introduction.md) | お題の説明と、すべてを画面に書いた Bad コード | － |
| [第1章 MVC](docs/part1/01-mvc.md) | Controller が入力を受け、Model を操作し、View を選ぶ | ASP.NET Core MVC |
| [第2章 MVP](docs/part1/02-mvp.md) | View をインターフェースにして、Presenter がすべてを受け持つ | WinForms |
| [第3章 MVVM](docs/part1/03-mvvm.md) | データバインディングで View と ViewModel をつなぐ | WPF + CommunityToolkit.Mvvm |
| [第4章 MVU](docs/part1/04-mvu.md) | 変更不可の状態と一方向のデータフロー | コンソール（自作） |
| [第5章 Flux / Redux](docs/part1/05-flux.md) | アプリ全体の状態を1か所の Store にまとめる | Blazor + Fluxor |
| [第1部まとめ](docs/part1/99-summary.md) | パターンの変化の流れと比較 | － |

### 第2部 アプリ全体のパターン

アプリ全体をどんな部品に分け、どの向きに依存させるか。

| 章 | 内容 | サンプル |
|---|---|---|
| [第6章 レイヤードアーキテクチャ](docs/part2/06-layered.md) | 表示・業務・データアクセスの3層 | ASP.NET Core Web API + EF Core |
| [第7章 クリーンアーキテクチャ](docs/part2/07-clean-architecture.md) | 依存の向きを内側にそろえる（コラム: オニオン / ヘキサゴナル） | 同上（4プロジェクト構成） |
| [第8章 DDD（ドメイン駆動設計）](docs/part2/08-ddd.md) | 値オブジェクト・エンティティ・集約 | ドメイン層 + テスト |
| [第9章 CQRS](docs/part2/09-cqrs.md) | 書き込みと読み取りを分ける | ASP.NET Core Web API + EF Core |
| [第10章 Vertical Slice Architecture](docs/part2/10-vertical-slice.md) | 層ではなく機能ごとにまとめる | 同上 |
| [第2部まとめ](docs/part2/99-summary.md) | 比較と選び方 | － |

## サンプルの動かし方

すべてのコマンドはこのフォルダ（`ProgramArch`）で実行します。

```bash
# 全体をビルド
dotnet build ProgramArch.slnx

# テストをすべて実行
dotnet test ProgramArch.slnx

# 1つのテストクラスだけ実行（例: MVP の Presenter のテスト）
dotnet test samples/Part1/Part1.Tests --filter "FullyQualifiedName~MvpPresenterTests"

# 各サンプルを起動
dotnet run --project samples/Part1/01-Mvc/Inventory.Mvc
dotnet run --project samples/Part1/02-Mvp/Inventory.Mvp.WinForms
dotnet run --project samples/Part1/03-Mvvm/Inventory.Mvvm.Wpf
dotnet run --project samples/Part1/04-Mvu/Inventory.Mvu
dotnet run --project samples/Part1/05-Flux/Inventory.Flux
dotnet run --project samples/Part2/06-Layered/Inventory.Layered.Presentation
dotnet run --project samples/Part2/07-CleanArchitecture/Inventory.Clean.WebApi
dotnet run --project samples/Part2/09-Cqrs/Inventory.Cqrs.WebApi
dotnet run --project samples/Part2/10-VerticalSlice/Inventory.VerticalSlice.WebApi
```

第2部の Web API は、各プロジェクトにある `.http` ファイルを Visual Studio や VS Code（REST Client 拡張）で開くと、リクエストをそのまま送れます。
SQLite のデータベースファイル（`*.db`）は初回起動時に自動で作られます。初期データに戻したいときは削除してください。

## フォルダ構成

```
ProgramArch/
├── docs/
│   ├── part1/        第1部の資料
│   └── part2/        第2部の資料
└── samples/
    ├── Part1/        第1部のサンプル（章ごとのフォルダ + Part1.Tests）
    └── Part2/        第2部のサンプル（章ごとのフォルダ + Part2.Tests）
```

Bad コードは資料の中にだけ載せています。サンプルフォルダにあるのは、各パターンで書き直した後のコードです。
