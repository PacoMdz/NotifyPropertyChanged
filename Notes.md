# Microsoft WPF DataContext Design

XMLNS Declarations
* xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
* xmlns:d="http://schemas.microsoft.com/expression/blend/2008"

Markup property
* mc:Ignorable="d"

DataContext Binding
* d:DataContext="{d:DesignInstance Type=vm:MainContext, IsDesignTimeCreatable=True}"
