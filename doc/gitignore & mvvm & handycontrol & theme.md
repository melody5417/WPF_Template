# .gitignore

.gitignore 使用的是 [github的模版](https://github.com/dotnet/wpf/blob/main/.gitignore).

# MvvmLight

wpf 提供了数据绑定的机制，比如使用接口 INotifyPropertyChanged 或者依赖属性 DependencyProperty。但是这两个机制实现双向绑定都要写很多模版代码，开发起来令人乏味且效率低。

调研发现 mvvmlight 这个库可以比较优雅简洁的实现数据双向绑定，还提供 Command 实现事件绑定，Messenger 实现 ViewModel 间的通信，能大幅提高开发效率，必须使用。

## 集成

通过 NuGet 安装 MvvmLight，快速又方便。

安装后会自动修改 App.xaml, 定义一个 vm 的命名空间。

```
<Application.Resources>
    <ResourceDictionary>
        <vm:ViewModelLocator x:Key="Locator" d:IsDataSource="True" xmlns:vm="clr-namespace:Demo.ViewModel" />
     </ResourceDictionary>
  </Application.Resources>
```

同时创建一个 ViewModel 文件夹，内部包含两个文件：ViewModelLocator 和 MainViewModel。

## 使用

### ViewModelLocator

默认创建的 ViewModelLocator 文件会报错，按照提示修改即可。

```
// ViewModelLocator.cs
public class ViewModelLocator
{

    public ViewModelLocator()
    {
        SimpleIoc.Default.Register<MainViewModel>();
    }

    public static ViewModelLocator instance = new ViewModelLocator();

    #region ViewModel

    public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();

    #endregion
}
```

### MainViewModel

MainViewModel 默认是 MainWindow 的 ViewModel，可以在 xaml 里进行绑定。
```
DataContext="{Binding Main, Source={StaticResource Locator}}"
```

在 ViewModel 里按照 MvvmLight 的 set 方法定义属性，即可实现双向绑定。因为 MvvmLight 在 set 方法里封装了 raisePropertyChanged 通知。

比如在 ViewModel 定义 Title 属性， xaml 文件即可绑定该属性，实现双向绑定。
```
// ViewModel
private string m_title;

public string Title
{
    get => m_title;
    set => Set(nameof(Title), ref m_title, value);
}
```

```
// xaml
<TextBlock Text="{Binding Title}"></TextBlock>
        
```

# HandyControl

写页面肯定得选一个 UI 组件库，毕竟原生组件真的有些不太行。最后选定 HandyControl，下载量很高，没什么纠结的直接用。

重点提一下 HandyControl 的代码实现非常棒，我的模版工程很多地方参考了 HandyControl 的实现。

## 集成

通过 NuGet 安装。

同时在 App.xaml 里配置引入 HandyControl 的主题等样式文件。

```
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/Theme.xaml" />
    <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml" />
</ResourceDictionary.MergedDictionaries>
```

## 使用

安装了 HandyControl 后就可以创建 HandyControl 定义组件的实例，使用 HandyControl 定义的样式。

首先引入 HandyControl 的命名空间 

```
xmlns:hc="https://handyorg.github.io/handycontrol" 
```

然后就可以创建组件，使用样式资源了。

```
// hc:InfoElement.Placeholder 为 HandyControl 对 TextBox 扩展的属性
<TextBox Text="{Binding InputText, UpdateSourceTrigger=PropertyChanged}"  hc:InfoElement.Placeholder="请输入内容，下方将显示预览" Style="{StaticResource TextBoxExtend}"/>

// TextBlockDefault 为 HandyControl 定义的样式
<TextBlock Text="{Binding InputText}" Style="{StaticResource TextBlockDefault}"></TextBlock>
```

# Theme

主题样式的实现是参考了 HandyControl, 在上一步配置 App.xaml 时发现引入了 HandyControl 的 Theme 文件，然后深入看了下发现这种主题样式的实现很棒，于是就拿来主义了~

整体实现思路是 统一定义样式到 Theme.xaml 文件，然后引入到 App.xaml 的全局 ResourceDictionary 里。

```
// App.xaml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="pack://application:,,,/Resources/Themes/Theme.xaml" />
</ResourceDictionary.MergedDictionaries>
```

```
// Theme.xaml
<!--命名规则采用**类型+描述**，类型在前的目标是方便智能补全-->

<!-- Color全局定义 命名规则 Color+Hex -->
<Color x:Key="Color58C2DB">#58C2DB</Color>
<Color x:Key="ColorEAF4F6">#EAF4F6</Color>

<!-- Brush全局定义 命名规则 Brush+描述 -->
<SolidColorBrush o:Freeze="True" x:Key="BrushBackground" Color="{StaticResource Color58C2DB}"/>
<SolidColorBrush o:Freeze="True" x:Key="BrushPanel" Color="{StaticResource ColorEAF4F6}"/>
。。。
```

```
// MainWindow.xaml 调用 
<StackPanel Background="{StaticResource BrushBackground}">
</StackPanel>
```


