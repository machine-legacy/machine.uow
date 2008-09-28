using System;
using System.Collections.Generic;

using Machine.Specifications;

namespace Machine.UoW.Specs
{
  public class when_manipulating_a_unit_of_work
  {
    protected static object instance;
    protected static UnitOfWork uow;
    protected static UnitOfWorkManagement unitOfWorkManagement;

    Establish context = () =>
    {
      instance = new object();
      unitOfWorkManagement = new UnitOfWorkManagement();
      uow = new UnitOfWork(unitOfWorkManagement);
    };
  }

  [Subject("Unit of work")]
  public class when_adding_a_null_reference : when_manipulating_a_unit_of_work
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() => uow.AddNew<object>(null));

    It should_fail = () =>
      error.ShouldBeOfType<ArgumentNullException>();
  }

  [Subject("Unit of work")]
  public class when_saving_a_null_reference : when_manipulating_a_unit_of_work
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() => uow.Save<object>(null));

    It should_fail = () =>
      error.ShouldBeOfType<ArgumentNullException>();
  }

  [Subject("Unit of work")]
  public class when_deleting_a_null_reference : when_manipulating_a_unit_of_work
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() => uow.Delete<object>(null));

    It should_fail = () =>
      error.ShouldBeOfType<ArgumentNullException>();
  }

  [Subject("Unit of work")]
  public class when_adding_object : when_manipulating_a_unit_of_work
  {
    Because of = () => uow.AddNew(instance);

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_added_change = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Added);
  }

  [Subject("Unit of work")]
  public class when_adding_object_multiple_times : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.AddNew(instance);
      uow.AddNew(instance);
    };

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_added_change = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Added);
  }

  [Subject("Unit of work")]
  public class when_saving_object : when_manipulating_a_unit_of_work
  {
    Because of = () => uow.Save(instance);

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_saved_change = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Saved);
  }

  [Subject("Unit of work")]
  public class when_saving_object_multiple_times : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.Save(instance);
      uow.Save(instance);
    };

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_saved_change = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Saved);
  }

  [Subject("Unit of work")]
  public class when_deleting_object : when_manipulating_a_unit_of_work
  {
    Because of = () => uow.Delete(instance);

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_deleted_change = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Deleted);
  }

  [Subject("Unit of work")]
  public class when_deleting_object_multiple_times : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.Delete(instance);
      uow.Delete(instance);
    };

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_deleted_change = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Deleted);
  }

  [Subject("Unit of work")]
  public class when_adding_and_then_removing_object : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.AddNew(instance);
      uow.Remove(instance);
    };

    It should_remove_the_object_from_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeFalse();
  }

  [Subject("Unit of work")]
  public class when_saving_and_then_removing_object : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.Save(instance);
      uow.Remove(instance);
    };

    It should_remove_the_object_from_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeFalse();
  }

  [Subject("Unit of work")]
  public class when_deleting_and_then_removing_object : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.Delete(instance);
      uow.Remove(instance);
    };

    It should_remove_the_object_from_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeFalse();
  }

  [Subject("Unit of work")]
  public class when_removing_object_that_has_no_entry : when_manipulating_a_unit_of_work
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() => uow.Remove(instance));

    It should_fail = () =>
      error.ShouldBeOfType<InvalidOperationException>();
  }

  [Subject("Unit of work")]
  public class when_adding_and_then_deleting_object : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.AddNew(instance);
      uow.Delete(instance);
    };

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_added_and_deleted_changes = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Added, UnitOfWorkChangeType.Deleted);
  }

  [Subject("Unit of work")]
  public class when_saving_and_then_deleting_object : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.Save(instance);
      uow.Delete(instance);
    };

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_saved_and_deleted_changes = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Saved, UnitOfWorkChangeType.Deleted);
  }

  [Subject("Unit of work")]
  public class when_saving_and_then_adding_object : when_manipulating_a_unit_of_work
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() =>
    {
      uow.Save(instance);
      uow.AddNew(instance);
    });

    It should_fail = () =>
      error.ShouldBeOfType<InvalidOperationException>();
  }

  [Subject("Unit of work")]
  public class when_adding_and_then_saving_and_then_deleting_object : when_manipulating_a_unit_of_work
  {
    Because of = () =>
    {
      uow.AddNew(instance);
      uow.Save(instance);
      uow.Delete(instance);
    };

    It should_put_the_object_in_the_entries = () =>
      uow.HasEntryFor(instance).ShouldBeTrue();

    It should_have_added_and_saved_and_deleted_changes = () =>
      uow.FindEntryFor(instance).Changes.ShouldContainOnly(UnitOfWorkChangeType.Added, UnitOfWorkChangeType.Saved, UnitOfWorkChangeType.Deleted);
  }

  [Subject("Unit of work")]
  public class when_deleting_and_then_saving_object : when_manipulating_a_unit_of_work
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() =>
    {
      uow.Delete(instance);
      uow.Save(instance);
    });

    It should_fail = () =>
      error.ShouldBeOfType<InvalidOperationException>();
  }

  [Subject("Unit of work")]
  public class when_deleting_and_then_adding_object : when_manipulating_a_unit_of_work
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() =>
    {
      uow.Delete(instance);
      uow.AddNew(instance);
    });

    It should_fail = () =>
      error.ShouldBeOfType<InvalidOperationException>();
  }
}
