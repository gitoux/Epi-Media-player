   M S B A M L   `   `   `  ����   Media player skin V2.0(    "Media_player_skin_V2._0.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$  
MainWindow��$	��650��$
��1000��$
��1000��$	��650�� � 	Resources    EMenu_close �� TargetProperty �� 
TargetName R� KeyTime  System.Windows.Visibility   Hidden R� Value   Visible<  6System.Windows.Media.Animation.EasingThicknessKeyFrame p� KeyTime	 p� Value  
EMenu_open  ECMenu_open  ECMenu_close  ESound_disable  ESound_enable  ELoop_enable  ELoop_disable��%v  &	        &	 Q    &	 �    &	 �    &	 �    &	 3    &	 �    &	 y    �� .��T� $ (UIElement.Visibility)�$ Menu_close_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Menu_open_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Playlist_menu_toolbar��..�i� $ 0��# Z r� $ (FrameworkElement.Margin)�$ Menu_video_grid��.	�  $ 0��$	 0q��� .��T� $ (UIElement.Visibility)�$ Menu_open_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Menu_close_button��..�i� $ 0��# Z r� $ (FrameworkElement.Margin)�$ Menu_video_grid��.	�  $ 0��$	 	201,0,0,0q�T� $ (UIElement.Visibility)�$ Playlist_menu_toolbar��..�i� $ 0��# Z �� .��T� $ (UIElement.Visibility)�$ CMenu_open_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ CMenu_close_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Extented_Menu��..�i� $ 0��# Z �� .��T� $ (UIElement.Visibility)�$ CMenu_close_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ CMenu_open_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Extented_Menu��..�i� $ 0��# Z �� .��T� $ (UIElement.Visibility)�$ Sound_on��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ 	Sound_off��..�i� $ 0��# Z �� .��T� $ (UIElement.Visibility)�$ 	Sound_off��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Sound_on��..�i� $ 0��# Z �� .��T� $ (UIElement.Visibility)�$ Loop_enable_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Loop_disable_button��..�i� $ 0��# Z �� .��T� $ (UIElement.Visibility)�$ Loop_disable_button��..�i� $ 0��# Z T� $ (UIElement.Visibility)�$ Loop_enable_button��..�i� $ 0��# Z 
 � Triggers
 :�  :� RoutedEvent$ ButtonBase.Click�� :� 
SourceName$ Menu_close_button��.[��� -    ��Name$! EMenu_close_BeginStoryboard��#��[  �� -   $# ECMenu_close_BeginStoryboard1��#��[ :� $ ButtonBase.Click��$ Menu_open_button��.[��� -   $  EMenu_open_BeginStoryboard��#��[ :� $ ButtonBase.Click��$ Full_screen_button��.[��� #��[  �� -   $# ECMenu_close_BeginStoryboard2��#��[ :� $ ButtonBase.Click��$ CMenu_open_button��.[��� -   $! ECMenu_open_BeginStoryboard��#��[ :� $ ButtonBase.Click��$ CMenu_close_button��.[��� -   $" ECMenu_close_BeginStoryboard��#��[ :� $ ButtonBase.Click��$ Sound_on��.[��� -   $$ ESound_disable_BeginStoryboard��#��[ :� $ ButtonBase.Click��$ 	Sound_off��.[��� -	   $# ESound_enable_BeginStoryboard��#��[ :� $ ButtonBase.Click��$ Loop_enable_button��.[��� -
   $" ELoop_enable_BeginStoryboard��#��[ :� $ ButtonBase.Click��$ Loop_disable_button��.[��� -   $# ELoop_disable_BeginStoryboard��#��[ :� $ ButtonBase.Click��$ Play_button��.[��� -   $# ECMenu_close_BeginStoryboard3��#��[ .��� -    �Name$ Global_grid��
�������$��0q�.+�� -   $ Playlist_menu_toolbar��$
��Left=�$	��197��
�������.+�R� -    R�Name$ Menu_library��$��-0.5,62.5,0.5,314.5q�
���   �
�������.@�Q� -    Q�Name$ PictureTree��$��Pictures��
�������Q� -   $ 	VideoTree��$��Video��
�������Q� -   $ 	MusicTree��$��Music��
�������.@�Q� -   $ 
ArtistTree��$��Artist��
�������Q� -   $ 	AlbumTree��$��Album��
�������Q� -   $ 	GenreTree��$��Genre��
�������R� -   $ treePl��
���   �$��216.5��$��0,0,0,94q�$��Bottom=�
�������� -   $ Toolbar_library��$��58��$	��Top=�
���   �$��0.5,0.5,0,0q�.+��� -    ��Name$ Update_button��$��35��$��35��$��35��$��8,8,153.5,15q�$��35��$��35��  TUpdate_button#���   J� RenderTransformOrigin$ 	1.6,0.686&�$��35���� -   $ Add_library_button��$��35��$��35��$��35��$��	47,8,0,15q�$��35��$��35�� 	 TGestion_lib_button#��� 	 $ 	1.6,0.686&�$
��Left=�$��39���� -   $ CMenu_open_button��$��35��$��35��$��35��$��153.5,8,8,15q�$��35��$��35�� 
 TCMenu_open_button#��� 
 $ 	1.6,0.686&�$��35��$y�Hidden=��� -   $ CMenu_close_button��$��35��$��35��$��35��$��153.5,8,8,15q�$��35��$��35��  TCMenu_close_button#���  $ 	1.6,0.686&�$��35��� -   $ Toolbar_playlist��$��90��$��Bottom=�
���   �$��	0,0,0.5,0q�.+��� -   $ Add_playlist_button��$��35��$��35��$��35��$��8,47,153.5,8q�$��35��$��35��  TAdd_playlist_button#���  $ 	1.6,0.686&��� -   $ Delete_playlist_button��$��35��$��35��$��35��$��105,47,56.5,8q�$��35��$��35��  TDelete_playlist_button#���  $ 	1.6,0.686&��� -   $ Add_media_button��$��35��$��35��$��35��$��8,8,153.5,47q�$��35��$��35��  TAdd_media_button#���  $ 	1.6,0.686&��� -    $ Delete_media_button��$��35��$��35��$��35��$��105,8,56.5,47q�$��35��$��35��  TDelete_media_button#���  $ 	1.6,0.686&�� -!   $ Menu_video_grid��$��	201,0,0,0q�$ 0.5,0.5&�z�V� .~��� �� �� T� .+�� -"   $ Toolbar_menu��
���   �$	��110��$��Bottom=�.+��� -#   $ Play_button��$
��Left=�$��45��$��45��$��45��$��
163,0,0,15q�$��45��$��45��  TPlay_button#���  $��45��$��Bottom=�$ 0.889,0.578&��� -$    ��Name$ 
Time_video��$��26��$��8,5,8,0q�$	��Top=�$��3���� -%   $ Break_button��$��45��$��45��$��45��$��
212,0,0,15q�$��45��$��45��  TBreak_button#���  $��Bottom=�$ 1.686,0.629&�$
��Left=��� -&   $ Fast_next_button��$��40��$��40��$��40��$��
261,0,0,15q�$��40��$��40��  TFast_next_button#���  $��Bottom=�$ 	-2.05,0.5&�$
��Left=��� -'   $ Fast_back_button��$
��Left=�$��40��$��40��$��40��$��	70,0,0,15q�$��40��$��40��  TFast_back_button#���  $��40��$��Bottom=�$ -0.171,0.514&��� -(   $ Next_button��$��35��$��35��$��35��$��
305,0,0,15q�$��35��$��35��  TNext_button#���  $��Bottom=�$ 	1.6,0.686&�$��35��$
��Left=��� -)   $ Back_button��$
��Left=�$��35��$��35��$��35��$��	31,0,0,15q�$��35��$��35��  TBack_button#���  $��35��$��Bottom=�$ 	1.6,0.686&��� -*   $ Stop_button��$
��Left=�$��45��$��45��$��45��$��114,50,0,15q�$��45��$��45��  TStop_button#���  $��45��$ 	1.6,0.686&��� -+   $ Loop_enable_button��$��Right=�$��40��$��40��$��40��$��
0,0,262,15q�$��40��$��40��  TLoop_enable_button#���  $��40��$��Bottom=�$ 	1.6,0.686&�$��2���� -,   $ Full_screen_button��$��40��$��40��$��
0,0,218,15q�$��40��$��40��  TFull_screen_button#���  $ 	1.6,0.686&�$��Right=�$��40��$��40��$��Bottom=��� --   $ 	Sound_off��$��35��$��35��$��35��$��
0,0,147,20q�$��35��$��35��  TOff_button#���  $��Bottom=�$ 	1.6,0.686&�$��Right=�$��35��$y�Hidden=��� -.   $ Sound_on��$��35��$��35��$��35��$��
0,0,147,20q�$��35��$��35��  
TOn_button#���  $��Bottom=�$ 	1.6,0.686&�$��Right=�$��35���� -/   $ Sound_settings��$��Right=�$��0,0,8,22q�$	��135�� � Minimum$ 0Q� � Maximum$ 1Q� � Value$	 0.5Q�$��28��$��Bottom=��� -0   $ Menu_close_button��$��80��$��40��$��376.182,0,327.818,15q�$��80��$��40��  TMenu_close_button#���  $ 	1.6,0.686&�$��40��$��Bottom=��� -1   $ Menu_open_button��$��80��$��40��$��376.182,0,327.818,15q�$��80��$��40��  TMenu_open_button#���  $ 	1.6,0.686&�$��40��$��Bottom=�$y�Hidden=��� -2    ��Name$ PlayingMedia��$��17��$
��Left=�$��
305,31,0,0q�$��Titre du média��$	��Top=�$	��229��
�������$
��Bold"��� -3   $ Loop_disable_button��$��Right=�$��40��$��40��$��40��$��
0,0,262,15q�$��40��$��40��  TLoop_disable_button#���  $��40��$��Bottom=�$ 	1.6,0.686&�$y�Hidden=�� -4   $ 
Video_Grid��$��	0,0,0,114q�
���   �.+�r� -5    r�Name$ MediaPlayer�� r� LoadedBehavior$ Manual=� r� Position$ 00:00:00h�$��1��� -6   $ Extented_Menu��$��0,0,147,114q�
���   �.+��� -7    ��Name$ listViewMedia��
���   �
�������