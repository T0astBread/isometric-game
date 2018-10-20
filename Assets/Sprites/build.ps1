# Delete previously generated sprites
rm .\Generated\SingleFiles\*

# Back the old spritesheet up (in case something goes wrong and you need to roll back)
rm -r .\Generated\Backup
mkdir .\Generated\Backup
mv .\Generated\spritesheet.png .\Generated\Backup\spritesheet.png
mv .\Generated\spritesheet.json .\Generated\Backup\spritesheet.json

# Save the Aseprite files as .png
foreach ($file in ls .\AsepriteFiles\) {
	$file -match "(.+)\.aseprite"  # Match against a file name regex
	$base_file_name = $Matches[1]  # Get the file name without extension

	Aseprite.exe -b .\AsepriteFiles\$file --save-as .\Generated\SingleFiles\$base_file_name".png"
}

# Build the spritesheet
sleep 2  # To accommodate for Aseprite CLI oddities
Aseprite.exe -b .\Generated\SingleFiles\* --sheet .\Generated\spritesheet.png --data .\Generated\spritesheet.json
