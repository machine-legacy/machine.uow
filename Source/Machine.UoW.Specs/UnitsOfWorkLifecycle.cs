using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Machine.Specifications;
using Rhino.Mocks;

namespace Machine.UoW.Specs
{
  [Subject("Unit of work creation")]
  public class when_creating_a_new_unit_of_work
  {
    static UnitOfWorkFactory factory;
    static UnitOfWork uow;

    Establish context = () =>
    {
      factory = new UnitOfWorkFactory(new UnitOfWorkManagement());
    };

    Because of = () =>
    {
      uow = (UnitOfWork)factory.StartUnitOfWork();
    };

    It should_have_no_entries = () =>
      uow.Entries.ShouldBeEmpty();
  }

  [Subject("Rolling back unit of work")]
  public class when_rolling_back_a_unit_of_work_with_no_applicable_events
  {
    static MockRepository mocks;
    static UnitOfWork uow;
    static UnitOfWorkManagement unitOfWorkManagement;
    static IUnitOfWorkEvents events;
    static object instance;
    static Exception error;

    Establish context = () =>
    {
      mocks = new MockRepository();
      events = mocks.Stub<IUnitOfWorkEvents>();
      unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(events);
      uow = new UnitOfWork(unitOfWorkManagement);
      instance = new object();
    };
    
    Because of = () => error = Catch.Exception(() =>
    {
      mocks.ReplayAll();
      uow.AddNew(instance);
      uow.Rollback();
    });

    It should_fail = () =>
      error.ShouldBeOfType<InvalidOperationException>();
  }

  [Subject("Flushing unit of work")]
  public class when_committing_a_unit_of_work_with_no_applicable_events
  {
    static MockRepository mocks;
    static UnitOfWork uow;
    static UnitOfWorkManagement unitOfWorkManagement;
    static IUnitOfWorkEvents events;
    static object instance;
    static Exception error;

    Establish context = () =>
    {
      mocks = new MockRepository();
      events = mocks.Stub<IUnitOfWorkEvents>();
      unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(events);
      uow = new UnitOfWork(unitOfWorkManagement);
      instance = new object();
    };
    
    Because of = () => error = Catch.Exception(() =>
    {
      mocks.ReplayAll();
      uow.AddNew(instance);
      uow.Commit();
    });

    It should_fail = () =>
      error.ShouldBeOfType<InvalidOperationException>();
  }

  public class with_events_for_committing_and_rolling_back
  {
    protected static MockRepository mocks;
    protected static UnitOfWork uow;
    protected static UnitOfWorkManagement unitOfWorkManagement;
    protected static IUnitOfWorkEvents events;
    protected static object added;
    protected static object deleted;
    protected static object saved;

    Establish context = () =>
    {
      mocks = new MockRepository();
      events = mocks.Stub<IUnitOfWorkEvents>();
      unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(events);
      uow = new UnitOfWork(unitOfWorkManagement);
      added = new object();
      saved = new object();
      deleted = new object();
      SetupResult.For(events.AppliesToObject(added)).Return(true);
      SetupResult.For(events.AppliesToObject(saved)).Return(true);
      SetupResult.For(events.AppliesToObject(deleted)).Return(true);
    };
  }

  [Subject("Rolling back unit of work")]
  public class when_rolling_back_a_unit_of_work : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.AddNew(added);
      uow.Save(saved);
      uow.Delete(deleted);
      uow.Rollback();
    };

    It should_call_rollback_for_all_objects = () =>
    {
      events.AssertWasCalled(x => x.Rollback(added));
      events.AssertWasCalled(x => x.Rollback(saved));
      events.AssertWasCalled(x => x.Rollback(deleted));
    };

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Rolling back unit of work")]
  public class when_rolling_back_a_unit_of_work_multiple_times : with_events_for_committing_and_rolling_back
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() =>
    {
      mocks.ReplayAll();
      uow.AddNew(added);
      uow.Save(saved);
      uow.Delete(deleted);
      uow.Rollback();
      uow.Rollback();
    });

    It should_fail = () => error.ShouldBeOfType<InvalidOperationException>();
  }
  
  [Subject("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_added_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.AddNew(added);
      uow.Commit();
    };

    It should_call_addnew = () => events.AssertWasCalled(x => x.AddNew(added));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_multiple_times_with_added_objects : with_events_for_committing_and_rolling_back
  {
    static Exception error;

    Because of = () => error = Catch.Exception(() =>
    {
      mocks.ReplayAll();
      uow.AddNew(added);
      uow.Save(saved);
      uow.Delete(deleted);
      uow.Commit();
      uow.Commit();
    });

    It should_fail = () => error.ShouldBeOfType<InvalidOperationException>();
  }
  
  [Subject("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_saved_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Save(added);
      uow.Commit();
    };

    It should_call_save = () => events.AssertWasCalled(x => x.Save(added));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_deleted_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Delete(added);
      uow.Commit();
    };

    It should_call_delete = () => events.AssertWasCalled(x => x.Delete(added));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_added_and_then_deleted_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.AddNew(added);
      uow.Delete(added);
      uow.Commit();
    };

    It should_call_nothing = () =>
    {
      events.AssertWasNotCalled(x => x.AddNew(added));
      events.AssertWasNotCalled(x => x.Save(added));
      events.AssertWasNotCalled(x => x.Delete(added));
    };

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_saved_and_then_deleted_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Save(added);
      uow.Delete(added);
      uow.Commit();
    };

    It should_call_delete = () => events.AssertWasCalled(x => x.Delete(added));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_added_and_then_saved_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.AddNew(added);
      uow.Save(added);
      uow.Commit();
    };

    It should_call_addnew_and_then_save = () =>
    {
      events.AssertWasCalled(x => x.AddNew(added));
      events.AssertWasCalled(x => x.Save(added));
    };

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
}
