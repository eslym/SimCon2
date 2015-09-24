#include <stdio.h>
#include "sc2.h"

void menu();
void menu_north();

void(*scene)();

int main(int argc, char * argv[]){
	if (argc > 0) sc2_title(argv[0]);
	else sc2_title("Untitled");
	scene = &menu;
	while (scene) {
		scene();
	}
}

void menu(){
	sc2m_reset();
	sc2m_add("Go to North", NULL, NULL);
	sc2m_add("Go to South", NULL, NULL);
	sc2m_add("Go to East", NULL, NULL);
	sc2m_add("Go to West", NULL, NULL);
	sc2m_add("Go Home", NULL, NULL);
	switch (sc2m_show("Where are you going?")){
	case 0:
		scene = &menu_north;
		break;
	case 1: case 2: case 3:
		printf("This way was deprecated, please go North\n");
		printf("\nPress any key to continue...");
		sc2_getkey(1);
		break;
	case 4:
		printf("Good Bye!\n");
		printf("\nPress any key to continue...");
		sc2_getkey(1);
		scene = NULL;
		break;
	}
}

void menu_north(){
	sc2m_reset();
	sc2m_add("Go Home", NULL, NULL);
	sc2m_add("Go Back", NULL, NULL);
	switch (sc2m_show("You are at north, but this way is nothing.")){
	case 0:
		printf("Good Bye!\n");
		printf("\nPress any key to continue...");
		sc2_getkey(1);
		scene = NULL;
		break;
	case 1:
		scene = &menu;
		break;
	}
}