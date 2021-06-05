var LibraryFixAudio = {
    FixAudio__proxy: 'sync',
    FixAudio__sig: 'v',
    FixAudio: function() {
	console.log("FixedAudio--------------" + WEBAudio.audioWebEnabled);
	console.log("FixedAudio----11----------" + WEBAudio.audioContext.state);
        WEBAudio.audioContext.resume();
    },
};
mergeInto(LibraryManager.library, LibraryFixAudio);