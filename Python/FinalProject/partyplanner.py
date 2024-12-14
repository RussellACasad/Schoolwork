#!/usr/bin/python3

# Party Planner Software, by Russell Casad
# Last Edits made April 22, 2023

import saveLoad
import UIList
import FoodMenu
import locale
locale.setlocale(locale.LC_ALL, 'en_us')

file_name = "partyplanner.csv"
app_name = "Party Planner"


# Used to add a guest to the attendee list, guests are tied to members
def add_guest(member_name, guest_list, index_to_add=None):

    # Initialize variables
    is_guest_paid = False

    if index_to_add is None:
        index_to_add = len(guest_list)

    # Inputs guest info
    while True:
        guest_name = input("Guest name: ").strip().title()
        if guest_name == "**":
            return
        elif guest_name.strip() == "":
            print("Guest name can not be blank.")
        else:
            break
    guest_menu_choice = FoodMenu.menu_order(f"Food Choice for {guest_name}")
    if guest_menu_choice == "**":
        return
    is_guest_paid_yn = input(f"Has {guest_name} paid? [Y/n]: ")
    if is_guest_paid_yn.lower() == "y":
        is_guest_paid = "Paid"
    elif is_guest_paid_yn.lower() == "n":
        is_guest_paid = "Unpaid"
    elif is_guest_paid_yn == "**":
        return

    if not guest_list:
        guest_list.append([guest_name, guest_menu_choice, is_guest_paid, "Guest", member_name])
    else:
        guest_list.insert(index_to_add, [guest_name, guest_menu_choice, is_guest_paid, "Guest", member_name])
    return guest_list


# Used to add a member to the attendee list
# guest list [name][menu choice][is paid?][member status][if guest, tied member]
def add_member(guest_list):

    # Returns to main program if menu is empty, attendees can't be added without menu items.
    menu_list = FoodMenu.menu_open()
    if not menu_list:
        print("╒════════════════════╕")
        print("│ Menu has no items! │")
        print("╘════════════════════╛")
        return

    while True:
        # Makes a UI for adding attendees or adding a guest to an attendee
        selection = UIList.ui_create("Add Attendees", ["Add Member", "Add Guest to Member"], 1, 0, 1, 1, 1)
        if selection == "1":    # Add member

            # Initialize variables for adding a member
            is_member_paid = False

            # Gets member info
            while True:
                member_name = input("Member name: ").strip().title()
                # Breaks loop if user inputs "**"
                if member_name == "**":
                    return
                elif member_name.strip() == "":
                    print("Member name can not be blank.")
                else:
                    break

            member_menu_choice = FoodMenu.menu_order(f"Food Choice for {member_name}")

            # Breaks loop if user inputs "**"
            if member_menu_choice == "**":
                return
            is_member_paid_yn = input(f"Has {member_name} paid? [Y/n]: ")
            if is_member_paid_yn.lower() == "y":
                is_member_paid = "Paid"
            elif is_member_paid_yn.lower() == "n":
                is_member_paid = "Unpaid"
            elif is_member_paid_yn == "**":
                return
            else:
                print("Invalid Response, Try Again.")

            # adds member
            has_guest_yn = input(f"Does {member_name} have any guests? [Y/n]: ").lower()
            if has_guest_yn == "y":
                guest_list.append([member_name, member_menu_choice, is_member_paid, "Member", "Null"])
                while True:
                    guest_list = add_guest(member_name, guest_list)
                    again = input(f"Does {member_name} have another guest? [Y/n]: ").lower()
                    if again == "y":
                        print("")
                    elif again == "n":
                        break
                    else:
                        print("Invalid Response, Try Again.")
            elif has_guest_yn == "n":
                guest_list.append([member_name, member_menu_choice, is_member_paid, "Member", "Null"])
                return
            elif has_guest_yn == "**":
                while True:
                    save_member = input(f"Save {member_name}? [Y/n]: ").lower()
                    if save_member == "y":
                        guest_list.append([member_name, member_menu_choice, is_member_paid, "Member", "Null"])
                        return guest_list
                    if save_member == "n":
                        return
                    else:
                        print("Invalid Response, Try again.")
            else:
                print("Invalid Response, Try Again.")

        if selection == "2":    # add guest to member

            if not guest_list:
                print("╒═══════════════════════════╕")
                print("│ No Members are attending! │")
                print("╘═══════════════════════════╛")
                return

            # initialize variables
            member_name_list = []
            index = -1

            # makes a list of all members
            for x in range(len(guest_list)):
                if guest_list[x][3] == "Member":
                    member_name_list.append(guest_list[x][0])

            if not member_name_list:
                print("╒═══════════════════════════╕")
                print("│ No Members are attending! │")
                print("╘═══════════════════════════╛")
                return

            # makes a ui of all members, and accepts a numerical input
            add_guest_selection = UIList.ui_create("Who's guest?", member_name_list, 1, 0, 1, 1, 1)
            if add_guest_selection == "**":
                return
            member_name = member_name_list[int(add_guest_selection) - 1]

            # gets the index of the member in the guest list and passes it to the add guest function so
            # members and their guests are listed together always
            for x in range(len(guest_list)):
                if guest_list[x][0] == member_name:
                    index = x + 1
                    break
            add_guest(member_name, guest_list, index)

        if selection == "**":
            break


# Used to edit attendees
def edit_attendee(guest_list):

    if not guest_list:
        print("╒═══════════════════╕")
        print("│ Guest List Empty! │")
        print("╘═══════════════════╛")
        return

    while True:
        attendee_to_edit = UIList.ui_create("Edit which attendee", guest_list, 1, 0, 1, 1, 1)
        if attendee_to_edit == "**":
            return
        print(f"Edit what part of {guest_list[int(attendee_to_edit) - 1][0]}")
        part_to_edit = UIList.ui_create(f"Edit what part of {guest_list[int(attendee_to_edit) - 1][0]}",
                                        ["Name", "Food Choice", "Paid Status"], 1, 0, 1, 1, 1)

        # Sets the attendee to edit variable to the index of the person
        attendee_to_edit = int(attendee_to_edit) - 1

        if part_to_edit == "1":     # edit name of attendee
            old_name = guest_list[attendee_to_edit][0]
            while True:
                # gets the new name of the attendee
                new_name = input(f"Input new name for {old_name}: ").strip().title()
                # checks the new name if it's blank
                if new_name == "":
                    print("Name can not be blank.")
                else:
                    break

            # checks if old name equals the new name, and responds as such
            if old_name == new_name:
                print("No Changes Made.")
            elif new_name == "**":
                print("No Changes Made.")
            else:
                guest_list[attendee_to_edit][0] = new_name
                print(f"Attendee {old_name} renamed to {new_name}.")

        # edits the food choice of an attendee
        elif part_to_edit == "2":   # Food Choice
            name = guest_list[attendee_to_edit][0]
            old_food = guest_list[attendee_to_edit][1]
            new_food = FoodMenu.menu_order(f"New food choice for {name}")
            if old_food == new_food:
                print("No Changes Made.")
            elif new_food == "**":
                print("No Changes Made.")
            else:
                guest_list[attendee_to_edit][1] = new_food
                print(f"Changed {name}'s food choice from {old_food} to {new_food}.")

        # edits the paid status of an attendee
        elif part_to_edit == "3":   # Paid Status
            if guest_list[attendee_to_edit][2] == "Paid":
                paid_status = "Unpaid"
            else:
                paid_status = "Paid"

            change_paid_status_yn = input(f"Change {guest_list[attendee_to_edit][0]}'s "
                                          f"paid status to {paid_status}? [Y/n]: ").lower().strip()
            if change_paid_status_yn == "y":
                guest_list[attendee_to_edit][2] = paid_status
                print("Paid Status Changed")
            elif change_paid_status_yn == "n":
                print("No Changes Made.")
            else:
                print("Invalid Response, Try again.")

        elif part_to_edit == "**":
            return


# Used to remove an attendee, guests are automatically removed with their member
def remove_attendee(guest_list):

    if not guest_list:
        print("╒══════════════════════════╕")
        print("│ No guests in guest list! │")
        print("╘══════════════════════════╛")
        return

    # Initialize variable
    num = 1

    # prints a list of all guests and their statuses
    print("╒" + ("═" * 15) + "╤" + ("═" * 15) + "╕")
    print("│{:^15}│{:^15}│".format("Attendee", "Status"))
    print("╞" + ("═" * 15) + "╪" + ("═" * 15) + "╡")
    for x in range(len(guest_list)):
        person = str(num).zfill(2) + ". " + guest_list[x][0]
        status = guest_list[x][3]
        print("│{:<15}│{:<15}│".format(person, status))
        num += 1
    print("╘" + ("═" * 15) + "╧" + ("═" * 15) + "╛")

    while True:
        # gets the attendee to remove
        attendee_to_remove = input("Which attendee to remove?: ")

        if attendee_to_remove == "**":
            return

        else:
            try:
                int(attendee_to_remove)
            except ValueError:
                print("\n\nNon-numerical value entered, try again\n\n")
            else:
                if 0 >= int(attendee_to_remove) > len(guest_list):
                    print("Invalid Response, Try again.")
                else:

                    # ends the string differently to notify the user that members delete their guests too
                    if guest_list[int(attendee_to_remove) - 1][3] == "Member":
                        and_guests = " and their guests? "
                    else:
                        and_guests = "? "

                    while True:
                        # Confirms the removal of attendee(s)
                        confirm = input(
                            f"Are you sure you want to remove {guest_list[int(attendee_to_remove) - 1][0]}{and_guests}[Y/n]: ").lower()
                        if confirm == "n":
                            return
                        elif confirm == "y":
                            print(f"Removing {guest_list[int(attendee_to_remove) - 1][0]}")
                            break
                        else:
                            print("Invalid Response, Try again.")
                    break

    to_remove = int(attendee_to_remove) - 1

    # removes a guest
    if guest_list[to_remove][3] == "Guest":
        remove = guest_list[to_remove]
        guest_list.remove(remove)

    # removes a member, and all associated guests
    elif guest_list[to_remove][3] == "Member":
        index_list = []
        member_name = guest_list[to_remove][0]
        remove = guest_list[to_remove]
        guest_list.remove(remove)
        for o in range(len(guest_list)):
            if guest_list[o][4] == member_name:
                index_list.append(o)

        for x in index_list:
            guest_list.pop(x)


# Lists attendees, and all their information
def list_attendees(guest_list):
    if not guest_list:
        print("╒═══════════════════╕")
        print("│ Guest List Empty! │")
        print("╘═══════════════════╛")
        return

    print("╒════════════════════╤════════════════════╤════════════════════╤════════════════════╕")
    print("│{:^20}│{:^20}│{:^20}│{:^20}│".format("Name", "Menu Choice", "Paid Status", "Attendee Type"))
    print("╞════════════════════╪════════════════════╪════════════════════╪════════════════════╡")
    for x in range(len(guest_list)):
        print("│{:<20}│{:<20}│{:<20}│{:<20}│".format(guest_list[x][0], guest_list[x][1], guest_list[x][2], guest_list[x][3]))
    print("╘════════════════════╧════════════════════╧════════════════════╧════════════════════╛")


# Marks attendee's fees as paid, easier way to mark unpaid -> paid than editing, was going to delete but QoL go brrr
def mark_fees(guest_list):
    while True:
        unpaid_list = []
        for i in range(len(guest_list)):
            if guest_list[i][2] == "Unpaid":
                unpaid_list.append(guest_list[i][0])

        if not unpaid_list:
            print("╒═══════════════════╕")
            print("│ No Unpaid Guests! │")
            print("╘═══════════════════╛")
            break

        mark_unpaid = UIList.ui_create("Mark which guest as paid?", unpaid_list, 1, 0, 1, 1, 1)

        if mark_unpaid == "**":
            break

        to_change = int(mark_unpaid) - 1

        for x in range(len(guest_list)):
            if guest_list[x][0] == unpaid_list[to_change]:
                guest_list[x][2] = "Paid"


# Lists fees both paid and unpaid, paid or not of 0 = unpaid, 1 = paid
def list_paid_fees(guest_list, paid_or_not=0):
    menu_list = FoodMenu.menu_open()
    collected = 0.00
    paid_status = "Unpaid"
    message = "Fees Paid"

    if not guest_list:
        print("╒═══════════════════╕")
        print("│ Guest List Empty! │")
        print("╘═══════════════════╛")
        return

    # changes whether function looks for paid or unpaid depending on input variable, defaults to unpaid
    if paid_or_not == 0:
        paid_status = "Unpaid"
    if paid_or_not == 1:
        paid_status = "Paid"

    message = "Fees " + paid_status

    print("\n\n")
    print("╒" + ("═" * 77) + "╕")
    print("│ {:^75} │".format(message))
    print("╞" + ("═" * 25) + ("╤" + "═" * 25) * 2 + "╡")
    print("│{:<25}│{:<25}│{:<25}│".format("Guest Name", "Menu Choice", "Member Status"))
    print("╞" + ("═" * 25) + ("╪" + "═" * 25) * 2 + "╡")

    for x in range(len(guest_list)):
        if guest_list[x][2] == paid_status:
            for y in range(len(menu_list)):
                if menu_list[y][0] == guest_list[x][1]:
                    print("│{:<25}│{:<25}│{:<25}│".format(guest_list[x][0], guest_list[x][1], guest_list[x][3]))
                    collected = collected + float(menu_list[y][1])

    string = "Total fees " + paid_status.lower() + ": "

    print("╞═════════════════════════╪═════════════════════════╪═════════════════════════╛")
    print("│{:<25}│{:<25}│".format(string, locale.currency(float(collected))))
    print("╘═════════════════════════╧═════════════════════════╛")


def main():
    main_option_list = ["!*Modify Attendees*!", "Add Attendees", "Remove Attendees", "Edit Attendees",
                        "Mark Fees Paid", "!*Lists*!", "List Attendees", "List Paid Fees", "List Unpaid Fees",
                        "!*Menu Options*!", "Add Menu Item", "List Menu Items"]
    guest_list = saveLoad.load(file_name)
    while True:
        user_input = UIList.ui_create(app_name, main_option_list, 2, 0, 0, 1)

        if user_input == "1":
            add_member(guest_list)

        elif user_input == "2":
            remove_attendee(guest_list)

        elif user_input == "3":
            edit_attendee(guest_list)

        elif user_input == "4":
            mark_fees(guest_list)

        elif user_input == "5":
            list_attendees(guest_list)

        elif user_input == "6":
            list_paid_fees(guest_list, 1)

        elif user_input == "7":
            list_paid_fees(guest_list, 0)

        elif user_input == "8":
            FoodMenu.menu_add()

        elif user_input == "9":
            FoodMenu.list_menu()

        elif user_input == "**":
            saveLoad.save(file_name, guest_list)
            print("Changes Saved, Exiting program...")
            break

        elif user_input == "!*":
            while True:
                ensure = input("Are you sure you want to quit without saving? [Y/n]: ")
                if ensure.lower() == "y":
                    exit()
                elif ensure.lower() == "n":
                    print("Returning...")
                    break
                else:
                    print("Invalid Input, Try again.")
        elif user_input == "$$":
            saveLoad.save(file_name, guest_list)
            print("Changes Saved!")


main()
