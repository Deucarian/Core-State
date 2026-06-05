# Standalone Repository Selection

This sample demonstrates CoreState without GenericUIItems, APIHelper, ServiceLocator, or project-specific code.

Open `StandaloneRepositorySelection.unity` and enter Play Mode. The sample uses IMGUI buttons to:

- select fake repository items by key
- reject a missing key
- remove the selected item
- clear the repository
- show current selection and repository contents

All Unity-specific code lives in this sample folder. The CoreState runtime remains pure C#.
