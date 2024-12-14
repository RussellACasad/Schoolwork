#!/usr/bin/python3

# Basic UI Maker programmed by Russell Casad
# Make a basic UI using these inputs:
# app name = the name of the application
# app list = a list of all the options you want used in the app
#       if you put "****" as an option, it will add a segmenting bar to the options for easier sorting of the options
#       if you put "!*TEXT*! you will get a title bar with the text inside the stars as the title.
# exit type = how you want exit options to show up on your software
#   0 = no exit options
#   1 = just exit
#   2 = exit with option to save or not save work
# Index to read is in case you input a list of lists, which index needs to be read from that list, it defaults to 0
# Show Bottom Title Bar is to toggle the bar at the bottom of the title in case you want to have a titled bar underneath
#       the title box
# Show Bar Above Exit Options allows to toggle on a bar above the exit options, so you don't have to add a bar
#       to the end of the list inputted if you want that bar there.
# back or exit changes exit type 1 to show "back" instead of "exit" to not confuse the user
# default use of this app would be UIList.ui_create({title}, [option list], 1, 0, 1, 1, 0]

def ui_create(app_name="Undefined", app_list=None, exit_type=1, index_to_read=0, show_bottom_title_bar=1,
              show_bar_above_exit_options=0, back_or_exit=0):
    # Initialize Variables
    char_count = 29
    bar = "│"
    bar_top = "╒"
    bar_bot = "╘"
    bar_mid = "╞"
    space = " "
    if app_list is None:
        app_list = ["options", "not", "****", "defined"]

    # defaults size to a comfortable size, sets the bar to the default size
    if len(app_name) <= 29:
        bar_top = "╒═══════════════════════════════╕"
        bar_mid = "╞═══════════════════════════════╡"
        bar_bot = "╘═══════════════════════════════╛"

    # makes a bar the size of the app name, plus some extra space, so it doesn't feel tight. also makes a space variable
    #   to offset the right sidebar for the options
    else:
        char_count = len(app_name)
        for x in range(len(app_name) - 1):
            bar += "═"
            if x > 27:
                space += " "
        bar_top = bar_top + bar + "═══╕"
        bar_bot = bar_bot + bar + "═══╛"
        bar_mid = bar_mid + bar + "═══╡"

    while True:
        # starts printing the UI, prints the name with a default size of 29, but can expand with longer titles
        number_of_options = 0
        print("\n\n")
        print(bar_top)
        print("│ {:^29} │".format(app_name))

        # toggleable bottom title bar
        if show_bottom_title_bar == 1:
            print(bar_mid)

        num = 1     # number used for numbering options
        for x in range(len(app_list)):
            if app_list[x] != "****":
                if type(app_list[0]) == list:
                    # if the input is a list, prints the input in the given index, defaults to 0
                    print("│{:>3}. {:<25}{:<1}│".format(str(num).zfill(2), app_list[x][index_to_read], space))
                    num += 1
                else:
                    # detects if not a bar, but is a title bar by detecting "!*" start and "*!" end, removes the start
                    #       and end variables to make it look better, and prints a title bar in accordance to the size
                    #       of the UI as well as the length of the title.
                    if app_list[x].startswith("!*") and app_list[x].endswith("*!"):
                        title = app_list[x].replace('!*', '─ ').replace('*!', ' ─').title()

                        if (char_count - len(title)) % 2 == 0:
                            title_bar = "├─" + title + ("──" * int((char_count - len(title)) / 2) + "─┤")
                        else:
                            title_bar = "├─" + title + ("──" * int((char_count - len(title) + 1) / 2) + "┤")
                        print(title_bar)
                    else:
                        #
                        print("│{:>3}. {:<25}{:<1}│".format(str(num).zfill(2), app_list[x], space))
                        number_of_options += 1
                        num += 1
            else:
                print(bar_mid)

        # if exit type isn't 0 and the programmer asks for a bar to appear above the exit, then the bar appears
        if show_bar_above_exit_options == 1 and exit_type != 0:
            print(bar_mid)

        # prints exit type with no exit options, useful in a y/n scenario
        if exit_type == 0:
            print(bar_bot)

        # prints just "exit"
        elif exit_type == 1:
            if back_or_exit == 0:
                print("│ **. Exit                   {:<1}  │".format(space))
                print(bar_bot)
            elif back_or_exit == 1:
                print("│ **. Back                   {:<1}  │".format(space))
                print(bar_bot)
            else:
                print("You are using this exit software incorrectly, please resort to the manual")

        # prints exit with save options
        elif exit_type == 2:
            print("│ $$. Save                   {:<1}  │ ".format(space))
            print("│ !*. Exit without saving    {:<1}  │ ".format(space))
            print("│ **. Save and exit          {:<1}  │ ".format(space))
            print(bar_bot)
        else:
            print("|=====================================================================|")
            print("| Exit Undefined, 0 has no exit, 1 has just exit, 2 has save and exit |")
            print("|=====================================================================|")

        # print(number_of_options)    # DEBUG

        user_input = input("Make a selection: ")

        # allows exit options to be returned only if applicable
        if exit_type == 1:
            if user_input == "**":
                return user_input
        elif exit_type == 2:
            if user_input == "**":
                return user_input
            elif user_input == "!*":
                return user_input
            elif user_input == "$$":
                return user_input

        # error exception handling done here so i don't have to later :3
        try:
            int(user_input)
        except ValueError:
            print("╒═════════════════════════════════════════╕")
            print("│ Non-numerical value entered, try again. │")
            print("╘═════════════════════════════════════════╛")

        else:
            if int(user_input) > number_of_options or int(user_input) < 0:
                print("╒═══════════════════════════╕")
                print("│ Invalid Input, try again. │")
                print("╘═══════════════════════════╛")
            else:
                return user_input.lstrip("0")
