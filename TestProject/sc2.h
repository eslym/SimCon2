/*
	Simple Console Interface Library v2
	
	by 0nepeop1e
*/
#ifndef SIMCON2
#define SIMCON2

#ifdef _DEBUG
#pragma comment(lib, "..\\SimCon2\\bin\\Debug\\sc2.lib")
#else
#pragma comment(lib, "..\\SimCon2\\bin\\Release\\sc2.lib")
#endif

#define true 1
#define false 0

struct sc2_key{
	/// <summary>Boolean to represent ALT key</summary>
	unsigned short ALT : 1;
	/// <summary>Boolean to represent SHIFT key</summary>
	unsigned short SHIFT : 1;
	/// <summary>Boolean to represent ALT key</summary>
	unsigned short CTRL : 1;
	/// <summary>Key Code of the key</summary>
	short keyCode;
};

/// <summary>Struct to represent a key</summary>
typedef struct sc2_key sc2_key;

/// <summary>Boolean type</summary>
typedef _Bool bool;

/// <summary>
/// Set the code page using by SimCon2 and the console,
/// default code page using is system default.
/// </summary>
/// <returns><c>true</c> if success, <c>false</c> if the code page not
/// supported or invalid.</returns>
/// <param name="CodePage">Code Page Identifier</param>
bool _cdecl sc2_encoding(int CodePage);

/// <summary>
/// Get string input
/// </summary>
/// <returns><c>true</c> if not overflow.</returns>
/// <param name="output">where to store the data</param>
/// <param name="size">size of array</param>
bool _cdecl sc2_getstr(char * output, size_t size);

/// <summary>
/// Get password input
/// </summary>
/// <param name="output">where to store the data</param>
/// <param name="size">size of array</param>
/// <param name="mask">character for mask, can use a wide character</param>
void _cdecl sc2_getpwd(char* output, size_t size, wchar_t mask);

/// <summary>
/// Get wide string input
/// </summary>
/// <returns>true if not overflow.</returns>
/// <param name="output">where to store the data</param>
/// <param name="size">size of array</param>
bool _cdecl sc2_getwcs(wchar_t * output, size_t size);

/// <summary>
/// Reads the console input and stores it in a queue.
/// </summary>
void _cdecl sc2_store();

/// <summary>
/// Gets the amount of strings stored in queue.
/// </summary>
/// <returns>Amount of strings stored in queue.</returns>
int _cdecl sc2_getStoredCount();

/// <summary>
/// Gets the length of the next string in queue.
/// </summary>
/// <returns> Length of next string in queue. </returns>
size_t _cdecl sc2_getNextLen();

/// <summary>
/// Gets the length of the next string in queue as wide chars.
/// </summary>
/// <returns> Length of next string in queue. </returns>
size_t _cdecl sc2_getNextLen_w();

/// <summary>
/// Retrieve stored string as multibyte to buffer
/// Also dequeues it.
/// </summary>
/// <returns>true if not overflow.</returns>
/// <param name="buffer">where to store the data</param>
/// <param name="size">size of array</param>
bool _cdecl sc2_getstore(char * buffer, size_t size);

/// <summary>
/// Retrieve stored string as widechar to buffer
/// Also dequeues it.
/// </summary>
/// <returns>true if not overflow.</returns>
/// <param name="buffer">where to store the data</param>
/// <param name="size">size of array</param>
bool _cdecl sc2_getstore_w(wchar_t * buffer, size_t size);

/// <summary>
/// Get integer input
/// </summary>
/// <returns>true if user input a valid value.</returns>
/// <param name="output">where to store the data</param>
bool _cdecl sc2_getint(int * output);

/// <summary>
/// Get float input
/// </summary>
/// <returns>true if user input a valid value.</returns>
/// <param name="output">where to store the data</param>
bool _cdecl sc2_getfloat(float * output);

/// <summary>Get key pressed</summary>
/// <returns>The key pressed</returns>
/// <param name="hide"><c>true</c> to hide the key pressed
/// from console</param>
sc2_key _cdecl sc2_getkey(bool hide);

/// <summary>Compare 2 sc2_key</summary>
/// <returns><code>true</code> if both key have same
/// modifiers and key code</returns>
/// <param name="k1">Key to compare</param>
/// <param name="k2">Key to compare</param>
bool _cdecl sc2_keycmp(sc2_key k1, sc2_key k2);

/// <summary>
/// Confirm from user.
/// </summary>
/// <returns>true if yes</returns>
/// <param name="prompt">true to prompt the "(Y/N)?" and answer.</param>
bool _cdecl sc2_confirm(bool prompt);

/// <summary>
/// Set the console title
/// </summary>
/// <param name="title">New title for the console</param>
void _cdecl sc2_title(char * title);

/// <summary>
/// Clear the console screen
/// </summary>
void _cdecl sc2_clrscr();

/// <summary>
/// Beep sound!
/// </summary>
void _cdecl sc2_beep();

/// <summary>
/// Key Listener for SimCon2 Menu
/// </summary>
/// <returns>true to stop the menu</returns>
/// <param name="key">key pressed</param>
typedef bool (_cdecl *sc2m_keyListener)(sc2_key key);

/// <summary>
/// Callback function for SimCon2 Menu
/// </summary>
/// <returns>result of callback</returns>
/// <param name="index">index of selected item</param>
typedef int (_cdecl *sc2m_callback)(int index);

/// <summary>
/// Set the callback function which will apply to the items
/// which doesn't have callback.
/// </summary>
/// <param="callback">the callback function</param>
void _cdecl sc2m_set_callback(sc2m_callback callback);

/// <summary>
/// Add a keyhandler function to handle keypresses.
/// </summary>
/// <param="callback">the callback function, function returns nonzero if the menu is to be updated</param>
void _cdecl sc2m_add_keyListener(sc2_key key, sc2m_keyListener keyListener);

/// <summary>
/// Show menu with a specific title
/// Function returns return value from either callback
/// or keyListener
/// </summary>
/// <param="title">the title</param>
int _cdecl sc2m_show(char * title);

/// <summary>
/// Show menu with a specific title
/// </summary>
/// <param="title">the title</param>
int _cdecl sc2m_show_w(wchar_t * title);

/// <summary>
/// Show menu with a specific title
/// </summary>
/// <param="title">the title</param>
/// <param="index">initial selection index</param>
int _cdecl sc2m_show_idx(char * title, int index);

/// <summary>
/// Show menu with a specific title
/// </summary>
/// <param="title">the title</param>
/// <param="index">initial selection index</param>
int _cdecl sc2m_show_w_idx(wchar_t * title, int index);

/// <summary>
/// Add an item into menu
/// </summary>
/// <param="name">Item name</param>
/// <param="tips">Tips for item, set null to disable</param>
/// <param="callback">Callback when item selected, nullable</param>
void _cdecl sc2m_add(char * name, char * tips, sc2m_callback callback);

/// <summary>
/// Add an item into menu
/// </summary>
/// <param="name">Item name</param>
/// <param="tips">Tips for item, set null to disable</param>
/// <param="callback">Callback when item selected, nullable</param>
void _cdecl sc2m_add_w(wchar_t * name, wchar_t * tips, sc2m_callback callback);

/// <summary>
/// Insert an item into menu with specific index
/// </summary>
/// <param="index">the index</param>
/// <param="name">Item name</param>
/// <param="tips">Tips for item, set null to disable</param>
/// <param="callback">Callback when item selected, nullable</param>
void _cdecl sc2m_insert(int index, char * name, char * tips, sc2m_callback callback);

/// <summary>
/// Insert an item into menu with specific index
/// </summary>
/// <param="index">the index</param>
/// <param="name">Item name</param>
/// <param="tips">Tips for item, set null to disable</param>
/// <param="callback">Callback when item selected, nullable</param>
void _cdecl sc2m_insert_w(int index, wchar_t * name, wchar_t * tips, sc2m_callback callback);

/// <summary>
/// remove an item in the menu which is located at the index.
/// </summary>
/// <param="index">the index</param>
void _cdecl sc2m_remove(int index);

/// <summary>
/// remove all items and clear the callback.
/// </summary>
void _cdecl sc2m_reset();

#endif