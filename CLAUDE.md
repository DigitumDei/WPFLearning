# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
This is a WPF tabbed browser application built with .NET 8.0 that demonstrates modern WPF patterns and third-party library integration.

## Architecture
- **MVVM Pattern**: Uses Model-View-ViewModel architecture with proper data binding
- **ViewModels**: Located in `ViewModels/` directory
  - `MainViewModel`: Manages tab collection and tab operations
  - `BrowserViewModel`: Base class for browser functionality with Reactive Extensions
  - `TabViewModel`: Inherits from BrowserViewModel, represents individual browser tabs
- **Reactive Programming**: Uses System.Reactive (Rx.NET) for address input throttling and suggestions
- **Third-party Libraries**: 
  - MahApps.Metro for modern UI styling
  - Dragablz for draggable tab control
  - Microsoft.Web.WebView2 for web content rendering

## Key Components
- **MainWindow.xaml**: Main UI with TabablzControl for draggable tabs
- **TabablzControl**: Handles tab creation, closing, and drag-and-drop functionality
- **WebView2**: Embedded browser control for web content
- **DuckDuckGo Integration**: Search suggestions with throttled input using Rx.NET

## Development Commands
Since this is a standard .NET project, use these commands (requires .NET 8.0 SDK):

```bash
# Build the project
dotnet build

# Run the application
dotnet run

# Build in release mode
dotnet build --configuration Release

# Clean build artifacts
dotnet clean

# Restore NuGet packages
dotnet restore
```

## NuGet Dependencies
- **Dragablz**: Draggable tab control
- **MahApps.Metro**: Modern WPF UI framework
- **Microsoft.Web.WebView2**: WebView2 control for web content
- **System.Reactive**: Reactive Extensions for .NET

## Code Patterns
- **INotifyPropertyChanged**: All ViewModels implement this for data binding
- **RelayCommand**: Custom command implementation in BrowserViewModel
- **Reactive Streams**: Address input uses Rx.NET for throttling and distinct filtering
- **URI Handling**: Automatic protocol prefixing for web addresses

## Current Features
- Multi-tab browsing with draggable tabs
- Address bar with navigation
- DuckDuckGo search suggestions (throttled input)
- Tab management (add/close tabs)
- WebView2 integration for web content rendering