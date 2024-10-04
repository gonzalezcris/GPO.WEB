Imports System.ComponentModel
Imports System.Web.UI
Imports System.Drawing
Imports System.Text

<Assembly: TagPrefix("CustomLabel", "edi")> 

Namespace HTMLEditorControl

    Public Class HTMLEditor
        Inherits System.Web.UI.WebControls.WebControl
        Implements IPostBackDataHandler

#Region "Private Variable Declaration"

        Public Event TextChanged As EventHandler

        Private mWidth As Integer = 350                 'Editor width
        Private mHeight As Integer = 300                'Editor height
        Private mEmDlgWidth As Integer = 110            'Emotion dialog width (smilies)
        Private mEmDlgHeight As Integer = 140           'Emotion dialog Height (smilies)
        Private mIconsPath As String = "files/icons"    'Icon path
        Private mSmilesPath As String = "files/smiles"  'Smiles path
        Private mButtonBackColor As Color = ColorTranslator.FromHtml("#E7EFF7") 'ColorTranslator.ToHtml
        Private mTableBackColor As Color = ColorTranslator.FromHtml("#E7EFF7")
        Private mCellBackColor As Color = ColorTranslator.FromHtml("#E7EFF7")
        Private mColorFilePath As String = "files/html/ColorPicker.html"        'Color picker file
        Private mSmilesFilePath As String = "files/html/Smiles.html"            'Smiles file
        Private mText As String = Nothing

        'Controle para habilitar e desabilitas as opções do editor
        Private mVisivelImagem As Boolean = True
        Private mVisivelHyperLink As Boolean = True
        Private mVisivelCopiarColar As Boolean = True
        Private mVisivelJustificar As Boolean = True
        Private mVisivelTipoTexto As Boolean = True
        Private mVisivelTamanho As Boolean = True
        Private mVisivelFonte As Boolean = True
        Private mVisivelParagrafo As Boolean = True
        Private mVisivelCor As Boolean = True
        Private mVisivelLinha As Boolean = True
        Private mVisivelEditarTexto As Boolean = True
        Private mVisivelSubscricao As Boolean = True
        Private mVisivelNumeracao As Boolean = True
        Private mVisivelRecuo As Boolean = True
        Private mVisivelVerHTML As Boolean = True

#End Region

#Region "Public Properties"

        '------------------------------------------
        'Editor height
        Public Property EditorHeight() As Integer
            Get
                Return mHeight
            End Get
            Set(ByVal Value As Integer)
                mHeight = Value
            End Set
        End Property
        '------------------------------------------
        'Editor width
        Public Property EditorWidth() As Integer
            Get
                Return mWidth
            End Get
            Set(ByVal Value As Integer)
                mWidth = Value
            End Set
        End Property
        '------------------------------------------
        'Emotions width (smiles)
        Public Property EmotionsDialogueWidth() As Integer
            Get
                Return mEmDlgWidth
            End Get
            Set(ByVal Value As Integer)
                mEmDlgWidth = Value
            End Set
        End Property
        '------------------------------------------
        'Emotions heigth (smiles)
        Public Property EmotionsDialogueHeight() As Integer
            Get
                Return mEmDlgHeight
            End Get
            Set(ByVal Value As Integer)
                mEmDlgHeight = Value
            End Set
        End Property
        '------------------------------------------
        'Path to icons
        Public Property IconsPath() As String
            Get
                Return mIconsPath
            End Get
            Set(ByVal Value As String)
                mIconsPath = Value
            End Set
        End Property
        '------------------------------------------
        'Path to smiles
        Public Property SmilesPath() As String
            Get
                Return mSmilesPath
            End Get
            Set(ByVal Value As String)
                mSmilesPath = Value
            End Set
        End Property
        '------------------------------------------
        'Return css (read only)
        Public ReadOnly Property StyleSheet() As String
            Get
                Return Me.GenerateCSSCode()
            End Get
        End Property
        '------------------------------------------
        'Button background color
        Public Property ButtonBackColor() As Color
            Get
                Return mButtonBackColor
            End Get
            Set(ByVal Value As Color)
                mButtonBackColor = Value
            End Set
        End Property
        '------------------------------------------
        'Table cell background color
        Public Property TableBackColor() As Color
            Get
                Return mTableBackColor
            End Get
            Set(ByVal Value As Color)
                mTableBackColor = Value
            End Set
        End Property
        '------------------------------------------
        'Table cell background color
        Public Property TableCellBackColor() As Color
            Get
                Return mCellBackColor
            End Get
            Set(ByVal Value As Color)
                mCellBackColor = Value
            End Set
        End Property
        '------------------------------------------
        'Path for smiles folder
        Public Property SmilesPickerFilePath() As String
            Get
                Return mSmilesFilePath
            End Get
            Set(ByVal Value As String)
                mSmilesFilePath = Value
            End Set
        End Property
        '------------------------------------------
        'Path for color picker folder
        Public Property ColorPickerFilePath() As String
            Get
                Return mColorFilePath
            End Get
            Set(ByVal Value As String)
                mColorFilePath = Value
            End Set
        End Property
        '------------------------------------------
        Public Property Text() As String
            Get
                If Not IsNothing(mText) Then
                    Return CType(mText, String)
                End If
                Return ""
            End Get
            Set(ByVal Value As String)
                Value = Replace(Value, "\", "&#92")       '\ is a Special Character, Replace with \\
                Value = Replace(Value, "'", "&#146")       ' ' is cause of Error, replace with \'
                Value = Replace(Value, vbCrLf, " ")
                mText = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelImagem() As Boolean
            Get
                Return mVisivelImagem
            End Get
            Set(ByVal Value As Boolean)
                mVisivelImagem = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelHyperLink() As Boolean
            Get
                Return mVisivelHyperLink
            End Get
            Set(ByVal Value As Boolean)
                mVisivelHyperLink = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelCopiarColar() As Boolean
            Get
                Return mVisivelCopiarColar
            End Get
            Set(ByVal Value As Boolean)
                mVisivelCopiarColar = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelJustificar() As Boolean
            Get
                Return mVisivelJustificar
            End Get
            Set(ByVal Value As Boolean)
                mVisivelJustificar = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelTipoTexto() As Boolean
            Get
                Return mVisivelTipoTexto
            End Get
            Set(ByVal Value As Boolean)
                mVisivelTipoTexto = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelTamanho() As Boolean
            Get
                Return mVisivelTamanho
            End Get
            Set(ByVal Value As Boolean)
                mVisivelTamanho = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelFonte() As Boolean
            Get
                Return mVisivelFonte
            End Get
            Set(ByVal Value As Boolean)
                mVisivelFonte = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelParagrafo() As Boolean
            Get
                Return mVisivelParagrafo
            End Get
            Set(ByVal Value As Boolean)
                mVisivelParagrafo = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelCor() As Boolean
            Get
                Return mVisivelCor
            End Get
            Set(ByVal Value As Boolean)
                mVisivelCor = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelLinha() As Boolean
            Get
                Return mVisivelLinha
            End Get
            Set(ByVal Value As Boolean)
                mVisivelLinha = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelEditarTexto() As Boolean
            Get
                Return mVisivelEditarTexto
            End Get
            Set(ByVal Value As Boolean)
                mVisivelEditarTexto = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelSubscricao() As Boolean
            Get
                Return mVisivelSubscricao
            End Get
            Set(ByVal Value As Boolean)
                mVisivelSubscricao = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelNumeracao() As Boolean
            Get
                Return mVisivelNumeracao
            End Get
            Set(ByVal Value As Boolean)
                mVisivelNumeracao = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelRecuo() As Boolean
            Get
                Return mVisivelRecuo
            End Get
            Set(ByVal Value As Boolean)
                mVisivelRecuo = Value
            End Set
        End Property
        '------------------------------------------
        Public Property VisivelVerHTML() As Boolean
            Get
                Return mVisivelVerHTML
            End Get
            Set(ByVal Value As Boolean)
                mVisivelVerHTML = Value
            End Set
        End Property

#End Region

#Region "Private Functions"

        '------------------------------------------
        Private Function GenerateCSSCode() As String

            Dim nl As String = Environment.NewLine

            Dim CSS As New StringBuilder

            With CSS

                CSS = .Append(nl)
                CSS = .Append("<style> " & nl)
                CSS = .Append(".EditControl       {       " & nl)
                CSS = .Append("width:" & Me.mWidth & "px;               " & nl)
                CSS = .Append("height:" & Me.mHeight & "px;      }       " & nl)

                CSS = .Append(".tblTable          {       " & nl)
                CSS = .Append("width : " & Me.mWidth & "px;             " & nl)
                CSS = .Append("height: 30px;              " & nl)
                CSS = .Append("border:0;                  " & nl)
                CSS = .Append("cellspacing:0;             " & nl)
                CSS = .Append("cellpadding:0;             " & nl)
                CSS = .Append("background-color:" & ColorTranslator.ToHtml(mTableBackColor) & ";" & nl)
                CSS = .Append("                   }     " & nl)

                CSS = .Append(".butClass          {       " & nl)
                CSS = .Append("width:22;                  " & nl)
                CSS = .Append("height:22;                 " & nl)
                CSS = .Append("border: 0px solid;         " & nl)
                CSS = .Append("border-color: #D6D3CE ;    " & nl)
                CSS = .Append("background-color:" & ColorTranslator.ToHtml(mButtonBackColor) & ";" & nl)
                CSS = .Append("                   }       " & nl)

                CSS = .Append(".tdClass           {       " & nl)
                CSS = .Append("padding-left: 0px;         " & nl)
                CSS = .Append("padding-top:0px;           " & nl)
                CSS = .Append("background-Color: " & ColorTranslator.ToHtml(mCellBackColor) & "; }       " & nl)

                CSS = .Append("</style>" & nl)

            End With

            Return (CSS.ToString & nl)

        End Function
        '------------------------------------------
        Private Function GenerateSelONScript() As String

            Dim nl As String = Environment.NewLine

            Dim jsScript As New StringBuilder

            With jsScript

                .Append("<script language=""javascript"">" & nl)
                .Append("function selOn(ctrl) {" & nl)
                .Append("   var mImage " & nl)
                .Append("   switch(ctrl.id)" & nl)
                .Append("   {" & nl)
                .Append("       case 'imgCuston' :" & nl)
                .Append("           mImage = 'customtag_over.gif';" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgAbout' :" & nl)
                .Append("           mImage = 'about_over.gif';" & nl)
                .Append("           break; " & nl)
                .Append("       case 'imgBold' :" & nl)
                .Append("           mImage = 'bold_over.gif';" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgItalic' :" & nl)
                .Append("           mImage = 'italic_over.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgBoldItalic' :" & nl)
                .Append("           mImage = 'bolditalicunderline_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgUnderLine' :" & nl)
                .Append("           mImage = 'underline_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgStrikeThrough' :" & nl)
                .Append("           mImage = 'strikethrough_over.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgSpecialChar' :" & nl)
                .Append("           mImage ='specialchars_over.gif'" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgSuperScript' :" & nl)
                .Append("           mImage = 'superscript_over.gif'" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgSubScript' :" & nl)
                .Append("           mImage = 'subscript_over.gif'" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgSave':" & nl)
                .Append("           mImage = 'save_over.gif'" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgAlignLeft' :" & nl)
                .Append("           mImage = 'alignleft_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgAlignRight' :" & nl)
                .Append("           mImage = 'alignright_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgAlignCenter' :" & nl)
                .Append("           mImage = 'aligncenter_over.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgAlignJustify' :" & nl)
                .Append("           mImage = 'alignjustify_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgCut' :" & nl)
                .Append("           mImage = 'cut_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgCopy' :" & nl)
                .Append("           mImage = 'copy_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgPaste' :" & nl)
                .Append("           mImage = 'paste_over.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgImage' :" & nl)
                .Append("           mImage = 'image_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgLink' :" & nl)
                .Append("           mImage = 'link_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgLine' :" & nl)
                .Append("           mImage = 'line_over.gif';" & nl)
                .Append("	        break;	" & nl)
                .Append("       case 'imgUndo' :" & nl)
                .Append("           mImage = 'undo_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgRedo' :" & nl)
                .Append("           mImage = 'redo_over.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgOrderList' :" & nl)
                .Append("           mImage = 'orderedlist_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgUnOrderList' :" & nl)
                .Append("           mImage = 'unorderedlist_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgOutdent' :" & nl)
                .Append("           mImage = 'outdent_over.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgIndent' :" & nl)
                .Append("           mImage = 'indent_over.gif';" & nl)
                .Append("           break;			" & nl)
                .Append("       case 'imgHighLight' :" & nl)
                .Append("           mImage = 'highlight_over.gif';" & nl)
                .Append("	        break;	" & nl)
                .Append("	    case 'imgFontColor' :" & nl)
                .Append("	        mImage = 'fontcolor_over.gif';" & nl)
                .Append("	        break;	" & nl)
                .Append("	    case 'imgCustom' :" & nl)
                .Append("	        mImage = 'customtag_over.gif';" & nl)
                .Append("	        break;	" & nl)
                .Append("	    default :" & nl)
                .Append("           mImage = ctrl.src;" & nl)
                .Append("   }" & nl)
                .Append("   var mNewImage = new Image();" & nl)
                .Append("   mNewImage.src = '" & mIconsPath & "/' + mImage ;" & nl)
                .Append("   if (! (mImage == ctrl.src) ) " & nl)
                .Append("       ctrl.src = mNewImage.src;" & nl)
                .Append("   ctrl.style.cursor = 'hand';	" & nl)
                '.Append("   ctrl.style.borderColor = '#000000';" & nl)
                '.Append("   ctrl.style.backgroundColor = '#B5BED6';" & nl)
                .Append("}" & nl)
                .Append("</script>" & nl)

            End With

            Return (jsScript.ToString & nl)

        End Function
        '------------------------------------------
        Private Function GenerateSelOFFScript() As String

            Dim nl As String = Environment.NewLine

            Dim jsScript As New StringBuilder

            With jsScript

                .Append("<script language=""javascript"">" & nl)
                .Append("function selOff(ctrl)" & nl)
                .Append("{" & nl)

                .Append("   var mImage ;" & nl)
                .Append("   switch(ctrl.id)" & nl)
                .Append("   {" & nl)
                .Append("       case 'imgCuston' :" & nl)
                .Append("           mImage = 'customtag_off.gif';" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgAbout' :" & nl)
                .Append("           mImage = 'about_off.gif';" & nl)
                .Append("           break; " & nl)

                .Append("       case 'imgBold' :" & nl)
                .Append("		    mImage = 'bold_off.gif';" & nl)
                .Append("	        break;" & nl)
                .Append("       case 'imgItalic' :" & nl)
                .Append("           mImage = 'italic_off.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgBoldItalic' :" & nl)
                .Append("           mImage = 'bolditalicunderline_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgSuperScript' :" & nl)
                .Append("           mImage = 'superscript_off.gif'" & nl)
                .Append("           break;" & nl)
                .Append("		case 'imgSubScript' :" & nl)
                .Append("	        mImage = 'subscript_off.gif'" & nl)
                .Append("           break;" & nl)
                .Append("       case 'imgSpecialChar' :" & nl)
                .Append("           mImage = 'specialchars_off.gif'" & nl)
                .Append("           break;" & nl)
                .Append("		case 'imgSave':" & nl)
                .Append("	        mImage = 'save_off.gif'" & nl)
                .Append("           break;" & nl)
                .Append("		case 'imgUnderLine' :" & nl)
                .Append("	        mImage = 'underline_off.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgStrikeThrough' :" & nl)
                .Append("           mImage = 'strikethrough_off.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgAlignLeft' :" & nl)
                .Append("           mImage = 'alignleft_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgAlignRight' :" & nl)
                .Append("           mImage = 'alignright_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgAlignCenter' :" & nl)
                .Append("           mImage = 'aligncenter_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgAlignJustify' :" & nl)
                .Append("           mImage = 'alignjustify_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("		case 'imgCut' :" & nl)
                .Append("	        mImage = 'cut_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgCopy' :" & nl)
                .Append("           mImage = 'copy_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgPaste' :" & nl)
                .Append("			mImage = 'paste_off.gif';" & nl)
                .Append("	        break;			" & nl)
                .Append("       case 'imgImage' :" & nl)
                .Append("           mImage = 'image_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgLink' :" & nl)
                .Append("           mImage = 'link_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgLine' :" & nl)
                .Append("           mImage = 'line_off.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgUndo' :" & nl)
                .Append("           mImage = 'undo_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgRedo' :" & nl)
                .Append("           mImage = 'redo_off.gif';" & nl)
                .Append("           break;			" & nl)
                .Append("       case 'imgOrderList' :" & nl)
                .Append("           mImage = 'orderedlist_off.gif';" & nl)
                .Append("           break;				" & nl)
                .Append("       case 'imgUnOrderList' :" & nl)
                .Append("           mImage = 'unorderedlist_off.gif';" & nl)
                .Append("           break; 				" & nl)
                .Append("       case 'imgOutdent' :" & nl)
                .Append("           mImage = 'outdent_off.gif';" & nl)
                .Append("           break; 				" & nl)
                .Append("       case 'imgIndent' :" & nl)
                .Append("           mImage = 'indent_off.gif';" & nl)
                .Append("           break; 	" & nl)
                .Append("       case 'imgHighLight' :" & nl)
                .Append("           mImage = 'highlight_off.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("       case 'imgFontColor' :" & nl)
                .Append("           mImage = 'fontcolor_off.gif';" & nl)
                .Append("           break;	" & nl)
                .Append("	    case 'imgCustom' :" & nl)
                .Append("	        mImage = 'customtag_off.gif';" & nl)
                .Append("	        break;	" & nl)
                .Append("       default :" & nl)
                .Append("           mImage = ctrl.src;			" & nl)
                .Append("   }" & nl)
                .Append("   var mNewImage = new Image();" & nl)
                .Append("   mNewImage.src = '" & mIconsPath & "/' + mImage ;" & nl)
                .Append("   if (! (mImage == ctrl.src) ) " & nl)
                .Append("       ctrl.src = mNewImage.src;" & nl)
                '.Append("   ctrl.style.borderColor = '#D6D3CE';  " & nl)
                '.Append("   ctrl.style.backgroundColor = '#D6D3CE';" & nl)
                .Append("}" & nl)

                .Append("</script>" & nl)

            End With

            Return (jsScript.ToString & nl)

        End Function
        '------------------------------------------
        Private Function GenerateSelDown_UpScript() As String

            Dim nl As String = Environment.NewLine

            Dim jsScript As New StringBuilder

            With jsScript

                .Append("<script language=""javascript"">" & nl)
                .Append("function selDown(ctrl)" & nl)
                .Append("{" & nl)
                .Append("   ctrl.style.backgroundColor = '" & ColorTranslator.ToHtml(Me.mButtonBackColor) & "';" & nl)
                .Append("}" & nl)
                .Append("function selUp(ctrl)" & nl)
                .Append("{" & nl)
                .Append("   ctrl.style.backgroundColor = '" & ColorTranslator.ToHtml(Me.mButtonBackColor) & "';" & nl)
                .Append("}" & nl)
                .Append("</script>" & nl)

            End With

            Return (jsScript.ToString & nl)


        End Function
        '------------------------------------------
        Private Function GenerateCommandScript() As String

            Dim nl As String = Environment.NewLine

            Dim jsScript As New StringBuilder

            With jsScript

                .Append("<script language=""javascript"">" & nl)
                .Append("function doCommand_" & Me.ID & "(ctrl)" & nl)
                .Append("{" & nl)
                .Append("   //BackColor" & nl)
                .Append("   var mCommand, uInterface, vValue;" & nl)
                .Append("switch(ctrl.id)" & nl)
                .Append("{		" & nl)
                .Append("   case 'imgSuperScript' :" & nl)
                .Append("	    mCommand = 'superscript';			" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'imgSubScript' :" & nl)
                .Append("	    mCommand = 'subscript';			" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;" & nl)
                .Append("   case 'imgBold' :" & nl)
                .Append("	    mCommand = 'bold';			" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;" & nl)
                .Append("   case 'imgItalic' :" & nl)
                .Append("	    mCommand = 'italic';			" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;	" & nl)
                .Append("   case 'imgBoldItalic' :" & nl)
                .Append("	    doCommand (document.all['imgBold'])" & nl)
                .Append("	    doCommand (document.all['imgItalic'])" & nl)
                .Append("	    doCommand (document.all['imgUnderLine'])" & nl)
                '.Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;" & nl)
                .Append("   case 'imgUnderLine' :" & nl)
                .Append("	    mCommand = 'underline';			" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;	" & nl)
                .Append("   case 'imgStrikeThrough' :" & nl)
                .Append("	    mCommand = 'strikethrough';		" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;	" & nl)
                .Append("   case 'imgAlignLeft' :" & nl)
                .Append("	    mCommand = 'justifyleft';			" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;				" & nl)
                .Append("   case 'imgAlignRight' :" & nl)
                .Append("	    mCommand = 'justifyright';" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;				" & nl)
                .Append("   case 'imgAlignCenter' :" & nl)
                .Append("	    mCommand = 'justifycenter';" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;				" & nl)
                .Append("   case 'imgAlignJustify' :" & nl)
                .Append("	    mCommand = 'justifyfull';" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;				" & nl)
                .Append("   case 'imgCut' :" & nl)
                .Append("	    mCommand = 'cut';" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;				" & nl)
                .Append("   case 'imgCopy' :" & nl)
                .Append("	    mCommand = 'copy';" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;				" & nl)
                .Append("   case 'imgPaste' :" & nl)
                .Append("	    mCommand = 'paste';" & nl)
                .Append("	    uInterface = false;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;			" & nl)
                .Append("   case 'imgImage' :" & nl)
                .Append("	    //Note that if we set UserInterface to true and vValue to null, then" & nl)
                .Append("	    //A Dialogue will appear asking for Image location." & nl)
                .Append("	    mCommand = 'insertimage';" & nl)
                .Append("	    uInterface = true;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("	    break;				" & nl)
                .Append("	case 'imgLink' :" & nl)
                .Append("	    mCommand = 'createlink';" & nl)
                .Append("	    uInterface = true;" & nl)
                .Append("	    vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;				" & nl)
                .Append("	case 'imgLine' :" & nl)
                .Append("		mCommand = 'inserthorizontalrule';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;	" & nl)
                .Append("	case 'imgUndo' :" & nl)
                .Append("		mCommand = 'undo';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;				" & nl)
                .Append("	case 'imgRedo' :" & nl)
                .Append("		mCommand = 'redo';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;				" & nl)
                .Append("	case 'imgOrderList' :" & nl)
                .Append("		mCommand = 'insertorderedlist';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;				" & nl)
                .Append("	case 'imgUnOrderList' :" & nl)
                .Append("		mCommand = 'insertunorderedlist';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;				" & nl)
                .Append("	case 'imgOutdent' :" & nl)
                .Append("		mCommand = 'outdent';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;				" & nl)
                .Append("	case 'imgIndent' :" & nl)
                .Append("		mCommand = 'indent';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = null;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'lstStyle' :" & nl)
                .Append("		mCommand = 'formatblock';" & nl)
                .Append("		vValue = ctrl.options[ctrl.selectedIndex].value ;" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                'TODO List Font is not working, Check here
                .Append("	case 'lstFontSize' :" & nl)
                .Append("		mCommand = 'fontsize';" & nl)
                .Append("		vValue = ctrl.options[ctrl.selectedIndex].value ;" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'lstFont' :" & nl)
                .Append("		mCommand = 'fontname';" & nl)
                .Append("		vValue = ctrl.options[ctrl.selectedIndex].value ;			" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'lstColor' :" & nl)
                .Append("		mCommand = 'forecolor';" & nl)
                .Append("		vValue = ctrl.options[ctrl.selectedIndex].value ;			" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;		" & nl)
                .Append("	case 'imgDelete' :" & nl)
                .Append("		mCommand = 'delete';" & nl)
                .Append("		vValue = null;" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'imgPrint' :" & nl)
                .Append("		mCommand = 'delete';" & nl)
                .Append("		vValue = null;" & nl)
                .Append("		uInterface = true;						" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'imgSave' :" & nl)
                .Append("		mCommand = 'saveas';" & nl)
                .Append("		vValue = null;" & nl)
                .Append("		uInterface = true;						" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'imgCustom' :		" & nl)
                .Append("		var temp = table2.style.display;" & nl)
                .Append("		if (temp == 'none')" & nl)
                .Append("		{" & nl)
                .Append("			var mTemp1 = RTFEdit_" & Me.UniqueID & ".document.body.innerText;" & nl)
                .Append("			table1.style.display = 'inline';" & nl)
                .Append("			table2.style.display = 'inline';		" & nl)
                .Append("			RTFEdit_" & Me.UniqueID & ".document.body.innerHTML = mTemp1;" & nl)
                .Append("		}" & nl)
                .Append("	    else" & nl)
                .Append("		{				" & nl)
                .Append("			//First Set the  HTMLText in the TextBox" & nl)
                .Append("			var mTemp = RTFEdit_" & Me.UniqueID & ".document.body.innerHTML;" & nl)
                .Append("			table1.style.display = 'none';" & nl)
                .Append("			table2.style.display = 'none';	" & nl)
                .Append("			RTFEdit_" & Me.UniqueID & ".document.body.innerText = mTemp;	 " & nl)
                .Append("		}" & nl)
                '.Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;			" & nl)
                .Append("	case 'imgFontColor' :" & nl)
                .Append("		var oldcolor = GetEditBoxColor('forecolor');    		" & nl)
                .Append("		mCommand = 'forecolor';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = GetColorFromUser(oldcolor);" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'imgHighLight' : " & nl)
                .Append("		var oldcolor = GetEditBoxColor('backcolor');    		" & nl)
                .Append("		mCommand = 'backcolor';" & nl)
                .Append("		uInterface = false;" & nl)
                .Append("		vValue = GetColorFromUser(oldcolor);		" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                'TODO Insert Special Characters
                .Append("	case 'imgSpecialChar' :" & nl)
                .Append("		alert ('Special Characters will be provided soon.');" & nl)
                .Append("		return;    		" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("	case 'imgSmile' :" & nl)
                .Append("		var posX    = event.screenX;" & nl)
                .Append("		var posY    = event.screenY + 20;" & nl)
                .Append("		var screenW = screen.width;                                 // screen size" & nl)
                .Append("		var screenH = screen.height - 20;                           // take taskbar into account" & nl)
                .Append("		//if (posX + 232 > screenW) { posX = posX - 232 - 40; }       // if mouse too far right" & nl)
                .Append("		//if (posY + 164 > screenH) { posY = posY - 164 - 80; }       // if mouse too far down" & nl)
                .Append("		var wPosition   = 'dialogLeft:' +posX+ '; dialogTop:' +posY;" & nl)
                .Append("		var newimage = showModalDialog('" & Me.mSmilesFilePath & "', ''," & nl)
                .Append("	            'dialogWidth:" & Me.EmotionsDialogueWidth & "px; dialogHeight: " & Me.EmotionsDialogueHeight & "px; '" & nl)
                .Append("			+ 'resizable: no; help: no; status: no; scroll: no; '" & nl)
                .Append("			+ wPosition);		" & nl)
                '.Append("		var firstpart = location.href ;" & nl)
                '.Append("		var firstpart = firstpart.substr(0, firstpart.lastIndexOf('/'))" & nl)
                .Append("       if (newimage == null) return;" & nl)
                .Append("		newimage = '" & Me.mSmilesPath & "' + '/' + newimage;" & nl)
                .Append("		mCommand = 'insertimage';" & nl)
                .Append("		vValue = newimage;" & nl)
                .Append("		uInterface = false;						" & nl)
                .Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("		break ;" & nl)
                'TODO Insert About Dialogue
                .Append("	case 'imgAbout' :" & nl)
                .Append("		alert ('HTML Editor. V1.0');" & nl)
                '.Append("       doit_" & Me.ID & "(mCommand, uInterface, vValue);" & nl)
                .Append("		break;" & nl)
                .Append("}" & nl)
                .Append("} " & nl)

                .Append("function doit_" & Me.ID & "(mCommand, uInterface, vValue)" & nl)
                .Append("{" & nl)
                .Append("	RTFEdit_" & Me.UniqueID & ".focus();" & nl)
                .Append("	RTFEdit_" & Me.UniqueID & ".document.execCommand(mCommand, uInterface, vValue);" & nl)
                .Append("	RTFEdit_" & Me.UniqueID & ".focus();" & nl)
                .Append("} " & nl)

                .Append("function GetColorFromUser(oldcolor)" & nl)
                .Append("{" & nl)
                .Append("	var posX    = event.screenX;" & nl)
                .Append("	var posY    = event.screenY + 20;" & nl)
                .Append("	var screenW = screen.width;                                 // screen size" & nl)
                .Append("	var screenH = screen.height - 20;                           // take taskbar into account" & nl)
                .Append("	if (posX + 232 > screenW) { posX = posX - 232 - 40; }       // if mouse too far right" & nl)
                .Append("	if (posY + 164 > screenH) { posY = posY - 164 - 80; }       // if mouse too far down" & nl)
                .Append("	var wPosition   = 'dialogLeft:' +posX+ '; dialogTop:' +posY;" & nl)
                .Append("	var newcolor = showModalDialog('" & Me.mColorFilePath & "', oldcolor," & nl)
                .Append("	        'dialogWidth:238px; dialogHeight: 187px; '" & nl)
                .Append("	+ 'resizable: no; help: no; status: no; scroll: no; '" & nl)
                .Append("	+ wPosition);" & nl)
                .Append("	        return newcolor" & nl)
                .Append("}" & nl)
                .Append("function GetEditBoxColor(colorCommand)" & nl)
                .Append("{" & nl)
                .Append("	return DecimalToRGB(RTFEdit_" & Me.UniqueID & ".document.queryCommandValue(colorCommand));" & nl)
                .Append("}" & nl)

                .Append("function DecimalToRGB(value) {" & nl)
                .Append("	var hex_string = '';" & nl)
                .Append("	for (var hexpair = 0; hexpair < 3; hexpair++) {" & nl)
                .Append("   	var byte = value & 0xFF;            // get low byte" & nl)
                .Append("	    value >>= 8;                        // drop low byte" & nl)
                .Append("	    var nybble2 = byte & 0x0F;          // get low nibble (4 bits)" & nl)
                .Append("	    var nybble1 = (byte >> 4) & 0x0F;   // get high nibble" & nl)
                .Append("	    hex_string += nybble1.toString(16); // convert nibble to hex" & nl)
                .Append("	    hex_string += nybble2.toString(16); // convert nibble to hex" & nl)
                .Append("   }" & nl)
                .Append("	return hex_string.toUpperCase();" & nl)
                .Append("}" & nl)

                .Append("</script>" & nl)

            End With

            Return (jsScript.ToString & nl)

        End Function
        '------------------------------------------
        Private Function GenerateHTMLAreaBody() As String

            Dim body As New StringBuilder

            Dim nl As String = Environment.NewLine

            With body

                'Start of Main Table which will have three tables
                .Append("<table id=""table_" & Me.ID & """ align=""center"" class=""tblTable"" cellpadding=""0"" cellspacing=""0"">" & nl)
                .Append("<tr>" & nl)
                .Append("<td class=""tdClass"">" & nl)

                'Start Making Frist Table (ie First Row of Toolbar) & nl)
                .Append("			<table id=""table1"" class=""tblTable"">" & nl)
                .Append("				<tr>" & nl)

                If VisivelEditarTexto Then
                    .Append("					<td class=""tdClass""><img ID=""imgSave"" alt=""Salvar"" name=""imgSave"" class=""butClass"" src=""" & mIconsPath & "/save_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgUndo"" alt=""Desfazer"" name=""imgUndo"" class=""butClass"" src=""" & mIconsPath & "/undo_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgRedo"" alt=""Refazer"" name=""imgRedo"" class=""butClass"" src=""" & mIconsPath & "/redo_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                If VisivelSubscricao Then
                    .Append("					<td class=""tdClass""><img ID=""imgSubScript"" alt=""Subscrição"" name=""imgSubScript"" class=""butClass"" src=""" & mIconsPath & "/subscript_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgSuperScript"" alt=""Superscrição"" name=""imgSuperScript"" class=""butClass"" src=""" & mIconsPath & "/superscript_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                If VisivelNumeracao Then
                    .Append("					<td class=""tdClass""><img ID=""imgOrderList"" alt=""Numeração"" name=""imgOrderList"" class=""butClass"" src=""" & mIconsPath & "/orderedlist_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgUnOrderList"" alt=""Marcadores"" name=""imgUnOrderList"" class=""butClass"" src=""" & mIconsPath & "/unorderedlist_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                If VisivelRecuo Then
                    .Append("					<td class=""tdClass""><img alt=""Diminuir recuo"" ID=""imgOutdent"" name=""imgOutdent"" class=""butClass"" src=""" & mIconsPath & "/outdent_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img alt=""Aumentar recuo"" ID=""imgIndent"" name=""imgIndent"" class=""butClass"" src=""" & mIconsPath & "/indent_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                If VisivelParagrafo Then
                    .Append("					<td class=""tdClass""> <!-- Insert the Style List -->" & nl)
                    .Append("						<select id=""lstStyle"" width=30px onChange=""doCommand_" & Me.ID & "(this);"">" & nl)
                    .Append("							<option value=""paragraph"">Parágrafo</option>" & nl)
                    .Append("							<option value=""Heading 1"">H1</option>" & nl)
                    .Append("							<option value=""Heading 2"">H2</option>" & nl)
                    .Append("							<option value=""Heading 3"">H3</option>" & nl)
                    .Append("							<option value=""Heading 4"">H4</option>" & nl)
                    .Append("							<option value=""Heading 5"">H5</option>" & nl)
                    .Append("							<option value=""Heading 6"">H6</option>" & nl)
                    .Append("							<option value=""Heading 7"">H7</option>" & nl)
                    .Append("						</select>" & nl)
                    .Append("					</td>" & nl)
                End If

                If VisivelFonte Then
                    .Append("					<td class=""tdClass"">" & nl)
                    .Append("						<Select id=""lstFont"" name=""lstFont"" width=30px onChange=""doCommand_" & Me.ID & "(this);"">" & nl)
                    .Append("							<option value=""Arial"">Arial</option>" & nl)
                    .Append("							<option value=""Courier"">Courier</option>" & nl)
                    .Append("							<option value=""Sans Serif"">Sans Serif</option>" & nl)
                    .Append("							<option value=""Tahoma"">Tahoma</option>" & nl)
                    .Append("							<option value=""Times Roman"">Times Roman</option>" & nl)
                    .Append("							<option value=""Verdana"">Verdana</option>" & nl)
                    .Append("							<option value=""Wingdings"">Wingdings</option>" & nl)
                    .Append("						</Select>" & nl)
                    .Append("					</td>" & nl)
                End If

                If VisivelTamanho Then
                    .Append("					<td class=""tdClass"">" & nl)
                    .Append("						<select id=""lstFontSize"" onChange=""doCommand_" & Me.ID & "(this);"" width=30px>" & nl)
                    .Append("							<option value=1>Muito pequeno</option>" & nl)
                    .Append("							<option value=2>Pequeno</option>" & nl)
                    .Append("							<option value=3>Médio</option>" & nl)
                    .Append("							<option value=4>Grande</option>" & nl)
                    .Append("							<option value=5>Ampliado</option>" & nl)
                    .Append("							<option value=6>Muito grande</option>" & nl)
                    .Append("							<option value=7>Extra grande</option>" & nl)
                    .Append("						</select>" & nl)
                    .Append("					</td>" & nl)
                End If

                .Append("				</tr>" & nl)
                .Append("			</table>" & nl)


                'End of First Table
                .Append("</td>" & nl)
                .Append("</tr>" & nl)

                'Create Second Table Now

                .Append("	<tr>" & nl)
                .Append("		<td class=""tdClass"">" & nl)
                .Append("			<table id=""table2"" class=""tblTable"">" & nl)
                .Append("				<tr>" & nl)
                .Append("					<td class=""tdClass"">" & nl)

                If VisivelTipoTexto Then
                    .Append("					<td class=""tdClass""><img ID=""imgBold"" alt=""Negrito"" name=""imgBold"" class=""butClass"" src=""" & mIconsPath & "/bold_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgItalic"" alt=""Itálico"" name=""imgItalic"" class=""butClass"" src=""" & mIconsPath & "/italic_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgUnderLine"" alt=""Sublinhado"" name=""imgUnderLine"" class=""butClass"" src=""" & mIconsPath & "/underline_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgBoldItalic"" alt=""Negrito itálico sublinhado"" name=""imgBoldItalic"" class=""butClass"" src=""" & mIconsPath & "/bolditalicunderline_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgStrikeThrough"" alt=""Tachado"" name=""imgStrikeThrough"" class=""butClass"" src=""" & mIconsPath & "/strikethrough_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                .Append("					<td class=""tdClass""><img src=""" & mIconsPath & "/separator.gif""></td>" & nl)

                If VisivelJustificar Then
                    .Append("					<td class=""tdClass""><img ID=""imgAlignLeft"" alt=""Alinhar à esquerda"" name=""imgAlignLeft"" class=""butClass"" src=""" & mIconsPath & "/alignleft_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgAlignCenter"" alt=""Centralizado"" name=""imgAlignCenter"" class=""butClass"" src=""" & mIconsPath & "/aligncenter_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgAlignRight"" alt=""Alinhar à direita"" name=""imgAlignRight"" class=""butClass"" src=""" & mIconsPath & "/alignright_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgAlignJustify"" alt=""Justificado"" name=""imgAlignJustify"" class=""butClass"" src=""" & mIconsPath & "/alignjustify_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                .Append("					<td class=""tdClass""><img src=""" & mIconsPath & "/separator.gif""></td>" & nl)

                If VisivelCopiarColar Then
                    .Append("					<td class=""tdClass""><img ID=""imgCut"" alt=""Recortar"" name=""imgCut"" class=""butClass"" src=""" & mIconsPath & "/cut_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgCopy"" alt=""Copiar"" name=""imgCopy"" class=""butClass"" src=""" & mIconsPath & "/copy_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgPaste"" alt=""Colar"" name=""imgPaste"" class=""butClass"" src=""" & mIconsPath & "/paste_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                .Append("					<td class=""tdClass""><!--<img ID=""imgSpecialChar"" alt=""Special Character"" name=""imgSpecialChar"" class=""butClass"" src=""" & mIconsPath & "/specialchars_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>-->" & nl)
                .Append("					<td class=""tdClass""><img src=""" & mIconsPath & "/separator.gif""></td>" & nl)

                If VisivelImagem Then
                    .Append("					<td class=""tdClass""><img ID=""imgImage"" alt=""Inserir imagem"" name=""imgImage"" class=""butClass"" src=""" & mIconsPath & "/image_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                If VisivelHyperLink Then
                    .Append("					<td class=""tdClass""><img ID=""imgLink"" alt=""Hyper link"" name=""imgLink"" class=""butClass"" src=""" & mIconsPath & "/link_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                If VisivelLinha Then
                    .Append("					<td class=""tdClass""><img ID=""imgLine"" alt=""Inserir linha"" name=""imgLine"" class=""butClass"" src=""" & mIconsPath & "/line_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                If VisivelCor Then
                    .Append("					<td class=""tdClass""><img ID=""imgFontColor"" alt=""Cor da fonte"" name=""imgFontColor"" class=""butClass"" src=""" & mIconsPath & "/fontcolor_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                    .Append("					<td class=""tdClass""><img ID=""imgHighLight"" alt=""Realce"" name=""imgHighLight"" class=""butClass"" src=""" & mIconsPath & "/highlight_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                End If

                '.Append("					<td class=""tdClass""><img ID=""imgSmile"" alt=""Insert Smiles"" name=""imgSmile"" class=""butClass"" src=""" & mIconsPath & "/1.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)""></td>" & nl)
                .Append("				</tr>" & nl)
                .Append("			</table>" & nl)
                'End of Second Table

                .Append("		</td>" & nl)
                .Append("</tr>" & nl)

                'Create Third Table Now

                .Append("	<tr>" & nl)
                .Append("		<td class=""tdClass"">" & nl)
                .Append("			<table id=""table3"" class=""tblTable"">" & nl)
                .Append("				<tr>" & nl)
                .Append("					<td class=""tdClass"">" & nl)
                '.Append("						<Form Id=""frmEdit"" name=""frmEdit"">" & nl)
                '.Append("							<Body onload=""iniciaIframe_" & Me.ID & "()"">" & nl)
                .Append("							    <IFrame name=RTFEdit_" & Me.UniqueID & " ID=RTFEdit_" & Me.UniqueID & " class=""EditControl"" ></IFrame>" & nl)
                .Append("							    <Input type=""hidden"" name=" & Me.UniqueID & " ID=" & Me.UniqueID & " value='" & Text & "' >" & nl)
                '.Append("							</Body>" & nl)
                '.Append("						</Form>" & nl)
                .Append("					</td>" & nl)
                .Append("				</tr>" & nl)
                .Append("				<tr>" & nl)
                .Append("					<td align=""right"" class=""tdClass"">" & nl)

                If VisivelVerHTML Then
                    .Append("						<img alt=""Ver HTML"" ID=""imgCustom"" name=""imgCustom"" class=""butClass"" src=""" & mIconsPath & "/customtag_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)"">" & nl)
                End If

                '.Append("						<img alt=""About"" ID=""imgAbout"" name=""imgAbout"" class=""butClass"" src=""" & mIconsPath & "/about_off.gif"" onMouseOver=""selOn(this)"" onMouseOut=""selOff(this)"" onMouseDown=""selDown(this)"" onMouseUp=""selUp(this)"" onClick=""doCommand_" & Me.ID & "(this)"">" & nl)
                .Append("					</td>" & nl)
                .Append("				</tr>" & nl)
                .Append("			</table>" & nl)

                'End of Third Table, Now Close the top most table
                .Append("		</td>" & nl)
                .Append("	</tr>" & nl)
                .Append("</table>" & nl)

            End With

            'That's all, Now return the HTML String
            Return (body.ToString & nl)

        End Function
        '------------------------------------------
        Private Function GeneratePostBackScript() As String

            Dim nl As String = Environment.NewLine

            Dim jsScript As New StringBuilder

            With jsScript

                .Append("<script language=""javascript"" for=RTFEdit_" & Me.UniqueID & " event=""onblur"">" & nl)
                .Append("var temp = table2.style.display;" & nl)
                .Append("if (temp == 'none' ) " & nl)
                .Append("   " & Me.UniqueID & ".value = RTFEdit_" & Me.UniqueID & ".document.body.innerText;" & nl)
                .Append("else" & nl)
                .Append("   " & Me.UniqueID & ".value = RTFEdit_" & Me.UniqueID & ".document.body.innerHTML;" & nl)
                .Append("</Script>" & nl)

                .Append("<Script Language=""javascript"" for=RTFEdit_" & Me.UniqueID & " event=""onload"">" & nl)
                .Append("RTFEdit_" & Me.UniqueID & ".document.body.innerHTML = '" & Me.Text & "'" & nl)
                .Append("</script>" & nl)

            End With

            Return (jsScript.ToString & nl)

        End Function

#End Region

#Region "Protected Overrides Sub"

        '------------------------------------------
        'Convert the Simple IFrame to Editable TextBox
        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

            Dim nl As String = Environment.NewLine

            writer.Write(Me.GenerateHTMLAreaBody)
            writer.RenderBeginTag("Script")
            'writer.Write("function iniciaIframe_" & Me.ID & "(){" & nl)
            'writer.Write("if( document.all && ! navigator.userAgent.match(/opera/gi) ) {" & nl)
            writer.Write("  RTFEdit_" & Me.UniqueID & ".document.designMode='on'; " & nl)
            'writer.Write("  }" & nl)
            'writer.Write("else {" & nl)
            'writer.Write("  document.getElementById('RTFEdit_" & Me.UniqueID & "').contentDocument.designMode= 'on'; }" & nl)
            'writer.Write("}" & nl)
            writer.RenderEndTag()

        End Sub
        '-----------------------------------------------------
        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

            If (Not Page.ClientScript.IsClientScriptBlockRegistered("myStylesSheetScript")) Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "myStyleSheetScript", Me.GenerateCSSCode())
            End If

            If (Not Page.ClientScript.IsClientScriptBlockRegistered("myCommandScript")) Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "myCommandScript", Me.GenerateCommandScript())
            End If

            If (Not Page.ClientScript.IsClientScriptBlockRegistered("mySelUpDownScript")) Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "mySelUpDownScript", Me.GenerateSelDown_UpScript())
            End If

            If (Not Page.ClientScript.IsClientScriptBlockRegistered("myselOFFScript")) Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "myselOFFScript", Me.GenerateSelOFFScript())
            End If

            If (Not Page.ClientScript.IsClientScriptBlockRegistered("myselONScript")) Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "myselONScript", Me.GenerateSelONScript())
            End If

            If (Not Page.ClientScript.IsClientScriptBlockRegistered(Me.UniqueID & "_PostBackScript")) Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, Me.UniqueID & "_PostBackScript", Me.GeneratePostBackScript())
            End If

        End Sub

#End Region

#Region "Interface Implementation"

        '--------------------------------------
        Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
            Dim currentValue As String = Me.Text
            Dim postedValue As String = postCollection(postDataKey)
            If currentValue Is Nothing Or Not postedValue.Equals(currentValue) Then
                Me.Text = postedValue
                Return True
            End If
            Return False
        End Function
        '--------------------------------------
        Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
            OnTextChanged(EventArgs.Empty)
        End Sub
        '--------------------------------------
        Protected Overridable Sub OnTextChanged(ByVal e As EventArgs)
            RaiseEvent TextChanged(Me, e)
        End Sub

#End Region

    End Class

End Namespace

