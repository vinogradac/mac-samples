// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;

namespace MacDatabinding
{
	public partial class CollectionViewController : NSViewController
	{
		#region Private Variables
		private NSMutableArray _people = new NSMutableArray();
		#endregion

		#region Computed Properties
		[Export("personModelArray")]
		public NSArray People {
			get { return _people; }
		}

		public PersonModel SelectedPerson { get; private set; }

		public nint SelectionIndex {
			get { return (nint)PeopleArray.SelectionIndex; }
			set { PeopleArray.SelectionIndex = (ulong)value; }
		}
		#endregion

		#region Constructors
		public CollectionViewController (IntPtr handle) : base (handle)
		{
		}
		#endregion

		#region Override Methods
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Build list of employees
			AddPerson (new PersonModel ("Craig Dunn", "Documentation Manager", true));
			AddPerson (new PersonModel ("Amy Burns", "Technical Writer"));
			AddPerson (new PersonModel ("Joel Martinez", "Web & Infrastructure"));
			AddPerson (new PersonModel ("Kevin Mullins", "Technical Writer"));
			AddPerson (new PersonModel ("Mark McLemore", "Technical Writer"));
			AddPerson (new PersonModel ("Tom Opgenorth", "Technical Writer"));
			AddPerson (new PersonModel ("Larry O'Brien", "API Documentation Manager", true));
			AddPerson (new PersonModel ("Mike Norman", "API Documentor"));

			// Watch for the selection value changing
			PeopleArray.AddObserver ("selectionIndexes", NSKeyValueObservingOptions.New, (sender) => {
				// Inform caller of selection change
				try {
					SelectedPerson = _people.GetItem<PersonModel>((nuint)SelectionIndex);
				} catch {
					SelectedPerson = null;
				}
			});
		}

		public override void PrepareForSegue (NSStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			// Take action based on type
			switch (segue.Identifier) {
			case "EditorSegue":
				var editor = segue.DestinationController as PersonEditorViewController;
				editor.Presentor = this;
				editor.Person = SelectedPerson;
				break;
			}
		}
		#endregion

		#region Public Methods
		public void DeletePerson(NSWindow window) {
			if (SelectedPerson == null) {
				var alert = new NSAlert () {
					AlertStyle = NSAlertStyle.Critical,
					InformativeText = "Please select the person to remove from the list of people.",
					MessageText = "Delete Person",
				};
				alert.BeginSheet (window);
			} else {
				// Confirm delete
				var alert = new NSAlert () {
					AlertStyle = NSAlertStyle.Critical,
					InformativeText = string.Format("Are you sure you want to delete person `{0}` from the table?",SelectedPerson.Name),
					MessageText = "Delete Person",
				};
				alert.AddButton ("Ok");
				alert.AddButton ("Cancel");
				alert.BeginSheetForResponse (window, (result) => {
					// Delete?
					if (result == 1000) {
						RemovePerson(SelectionIndex);
					}
				});
			}
		}

		public void EditPerson(NSWindow window) {
			if (SelectedPerson == null) {
				var alert = new NSAlert () {
					AlertStyle = NSAlertStyle.Informational,
					InformativeText = "Please select the person to edit from the list of people.",
					MessageText = "Edit Person",
				};
				alert.BeginSheet (window);
			} else {
				// Display editor
				PerformSegue("EditorSegue", this);
			}
		}

		public void FindPerson(string text) {

			// Convert to lower case
			text = text.ToLower ();

			// Scan each person in the list
			for (nuint n = 0; n < _people.Count; ++n) {
				var person = _people.GetItem<PersonModel> (n);
				if (person.Name.ToLower ().Contains (text)) {
					SelectionIndex = (nint)n;
					return;
				}
			}

			// Not found, select none  
			SelectionIndex = 0;
		}
		#endregion

		#region Array Controller Methods
		[Export("addObject:")]
		public void AddPerson(PersonModel person) {
			WillChangeValue ("personModelArray");
			_people.Add (person);
			DidChangeValue ("personModelArray");
		}

		[Export("insertObject:inPersonModelArrayAtIndex:")]
		public void InsertPerson(PersonModel person, nint index) {
			WillChangeValue ("personModelArray");
			_people.Insert (person, index);
			DidChangeValue ("personModelArray");
		}

		[Export("removeObjectFromPersonModelArrayAtIndex:")]
		public void RemovePerson(nint index) {
			WillChangeValue ("personModelArray");
			_people.RemoveObject (index);
			DidChangeValue ("personModelArray");
		}

		[Export("setPersonModelArray:")]
		public void SetPeople(NSMutableArray array) {
			WillChangeValue ("personModelArray");
			_people = array;
			DidChangeValue ("personModelArray");
		}
		#endregion

	}
}