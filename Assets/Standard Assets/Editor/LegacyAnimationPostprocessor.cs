using UnityEngine;
using UnityEditor;
using System.IO;


public class LegacyAnimationPostProcessor : AssetPostprocessor {

    void OnPreprocessModel () {
	
		//filter unrelated assets
		if (!assetPath.Contains("@") || !assetPath.Contains("/Animations/")) {
			return;
		}		

        //filter again
		string asset_filename = Path.GetFileNameWithoutExtension(assetPath);
        string[] split_filenames = asset_filename.Split('@');
		ModelImporter model_importer = (ModelImporter) assetImporter;
		if (split_filenames.Length!=2 || model_importer.clipAnimations.Length!=1)
			return;
				
                
		model_importer.animationType = ModelImporterAnimationType.Legacy;

		//modify the clips
		ModelImporterClipAnimation first_clip = model_importer.clipAnimations[0];
		ModelImporterClipAnimation[] clips = new ModelImporterClipAnimation[1];
		clips[0] = first_clip;		
		
		first_clip.name = split_filenames[1];
		first_clip.firstFrame = 1;
		
		model_importer.clipAnimations = clips;
		
		
		

	}	
	
}
