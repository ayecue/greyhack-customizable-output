# greyhack-customizable-output

Grey Hack recently introduced a limit of **2048 characters per print line tag**, which can cause images (and other rich text outputs) to display incorrectly when the limit is exceeded. This means that large images or detailed outputs are no longer fully supported in vanilla scripting.

This small mod addresses that limitation by making the **allowed tag and text length configurable**. It does **not** bypass the overall line character limit of **160k**, which is enforced by the GreyScript interpreter itself.

By default, the mod restores the previous limits used prior to the update.

## Purpose

Enable larger custom outputs (e.g., images using tags) by configuring the max character length per tag and text block.

## Associated Changelog (Grey Hack update)

> **Fixed**: Bug that could cause a crash or unexpected shutdown when using certain tags in scripts within infinite loops.  
> Introduced in:
> - Public version: `v0.9.5683`  
> - Nightly version: `v0.9.5905E`

## How to install

Just drag and drop it into your BepInEx plugins folder.