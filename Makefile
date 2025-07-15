# Makefile for GreyHack Customizable Output projects

# Variables
DOTNET = dotnet
SOLUTION_DIR = .
BEPINEX5_DIR = bepinex5
BEPINEX6_DIR = bepinex6
BUILD_CONFIG = Release

# Default target
.PHONY: all
all: build-bepinex5 build-bepinex6

# Build BepInEx 5 project
.PHONY: build-bepinex5
build-bepinex5:
	@echo "Building BepInEx 5 project..."
	cd $(BEPINEX5_DIR) && $(DOTNET) build --configuration $(BUILD_CONFIG)

# Build BepInEx 6 project
.PHONY: build-bepinex6
build-bepinex6:
	@echo "Building BepInEx 6 project..."
	cd $(BEPINEX6_DIR) && $(DOTNET) build --configuration $(BUILD_CONFIG)

# Clean both projects
.PHONY: clean
clean: clean-bepinex5 clean-bepinex6

.PHONY: clean-bepinex5
clean-bepinex5:
	@echo "Cleaning BepInEx 5 project..."
	cd $(BEPINEX5_DIR) && $(DOTNET) clean

.PHONY: clean-bepinex6
clean-bepinex6:
	@echo "Cleaning BepInEx 6 project..."
	cd $(BEPINEX6_DIR) && $(DOTNET) clean

# Restore packages for both projects
.PHONY: restore
restore: restore-bepinex5 restore-bepinex6

.PHONY: restore-bepinex5
restore-bepinex5:
	@echo "Restoring packages for BepInEx 5 project..."
	cd $(BEPINEX5_DIR) && $(DOTNET) restore

.PHONY: restore-bepinex6
restore-bepinex6:
	@echo "Restoring packages for BepInEx 6 project..."
	cd $(BEPINEX6_DIR) && $(DOTNET) restore

# Rebuild both projects (clean + build)
.PHONY: rebuild
rebuild: clean all

# Debug builds
.PHONY: debug
debug: BUILD_CONFIG = Debug
debug: all

# Release builds (default)
.PHONY: release
release: BUILD_CONFIG = Release
release: all

# Help target
.PHONY: help
help:
	@echo "Available targets:"
	@echo "  all           - Build both BepInEx 5 and 6 projects (default)"
	@echo "  build-bepinex5 - Build only BepInEx 5 project"
	@echo "  build-bepinex6 - Build only BepInEx 6 project"
	@echo "  clean         - Clean both projects"
	@echo "  clean-bepinex5 - Clean only BepInEx 5 project"
	@echo "  clean-bepinex6 - Clean only BepInEx 6 project"
	@echo "  restore       - Restore packages for both projects"
	@echo "  rebuild       - Clean and build both projects"
	@echo "  debug         - Build both projects in Debug configuration"
	@echo "  release       - Build both projects in Release configuration"
	@echo "  help          - Show this help message"