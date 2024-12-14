#!/usr/bin/python3

# menu programmed by Russell Casad
# This module is for making an expandable menu with custom items, and prices
# This module saves the menu as a CSV for future use too

# Imports the needed software
import UIList
import saveLoad
import locale

# declares file name
file_name = "menu.csv"

# Set locale
locale.setlocale(locale.LC_ALL, 'en_us')


# opens te menu list csv for use
def menu_open():
    menu_list = saveLoad.load(file_name)
    return menu_list


# saves the menu list when needed
def menu_close(menu_list):
    saveLoad.save(file_name, menu_list)


# makes a list of menu items and returns the item called to tie to a person
def menu_order(title_text="undefined"):
    menu_list = menu_open()
    menu_choice = UIList.ui_create(title_text, menu_list, 1, 0, 1, 1, 1)

    if menu_choice == "**":
        return menu_choice

    item = menu_list[int(menu_choice) - 1][0]
    return item


# adds a menu item to the menu csv file
def menu_add():
    menu_list = menu_open()

    while True:
        print("Input \"**\" at any time to quit.")
        food_to_add = input("Food to add: ")
        if food_to_add == "**":
            break
        price_of_food = input("Price of food item: $")
        if price_of_food == "**":
            break
        try:
            float(price_of_food)
        except ValueError:
            print("\n\nNon-numerical value entered, try again.\n\n")
        else:
            menu_list.append([food_to_add, price_of_food])

            do_again = input("Add Another Item? [Y/n]: ")
            if do_again.lower() == "n":
                menu_close(menu_list)
                break
            elif do_again.lower() != "y":
                print(f"Invalid Response {do_again}, try again.")


# lists menu options and the price for them, prices are variable
def list_menu():
    menu_list = menu_open()
    print("╒═══════════════╤═══════════════╕")
    print("│{:^15}│{:^15}│".format("Food", "Price"))
    print("╞═══════════════╪═══════════════╡")

    for i in range(len(menu_list)):
        print("|{:<15}|{:<15}|".format(menu_list[i][0], locale.currency(float(menu_list[i][1]))))

    print("╘═══════════════╧═══════════════╛")
