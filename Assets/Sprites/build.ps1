# Back the old spritesheet up (in case something goes wrong and you need to roll back)
rm -r .\Backup
cp -Path .\Generated -Destination .\Backup -Container -Recurse

# Save the Aseprite files that belong to the tiles as .png
foreach ($file in ls .\AsepriteFiles\Tiles) {
	$file -match "(.+)\.aseprite"  # Match against a file name regex
	$base_file_name = $Matches[1]  # Get the file name without extension

	Aseprite.exe -b .\AsepriteFiles\Tiles\$file --save-as .\Generated\Tiles\$base_file_name".png"
}

# Build the tiles spritesheet
sleep 2  # To accommodate for Aseprite CLI oddities
Aseprite.exe -b .\Generated\Tiles\* --sheet .\Generated\Tiles_spritesheet.png --data .\Generated\Tiles_spritesheet.json

# Save the player sprite as a spritesheet
sleep 2
Aseprite.exe -b .\AsepriteFiles\player.aseprite --sheet .\Generated\player_spritesheet.png --data .\Generated\player_spritesheet.json
