# Microsoft WPF DataContext Design

XMLNS Declarations
* **xmlns:mc**="http://schemas.openxmlformats.org/markup-compatibility/2006"
* **xmlns:d**="http://schemas.microsoft.com/expression/blend/2008"

Markup property
* **mc:Ignorable**="d"

DataContext Binding
* **d:DataContext**="{d:DesignInstance **Type**=viewmodel:CustomViewModel, **IsDesignTimeCreatable**=True}"

## XAML Example

```xml
<window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    x:Class="SpaceName.ClassName"
        
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    mc:Ignorable="d"
        
    xmlns:viewmodel="clr-namespace:CustomViewModel"
    d:DataContext="{d:DesignInstance Type=viewmodel:CustomViewModel, IsDesignTimeCreatable=True}">

</window>
```
