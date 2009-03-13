using System;
using System.Collections.Generic;

using Machine.Specifications;
using Rhino.Mocks;

namespace Machine.UoW.Specs
{
  [Subject("Starting a unit of work")]
  public class when_creating_a_new_unit_of_work
  {
    static MockRepository mocks;
    static UnitOfWork uow;
    static UnitOfWorkManagement unitOfWorkManagement;
    static IUnitOfWorkEvents events;
    static UnitOfWorkFactory factory;

    Establish context = () =>
    {
      mocks = new MockRepository();
      events = mocks.Stub<IUnitOfWorkEvents>();
      unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(events);
      factory = new UnitOfWorkFactory(unitOfWorkManagement);
      uow = (UnitOfWork)factory.StartUnitOfWork(new NullScope());
    };

    It should_have_no_entries = () =>
      uow.Entries.ShouldBeEmpty();
  }

  [Subject("Starting a unit of work")]
  public class when_starting_a_new_unit_of_work
  {
    static MockRepository mocks;
    static UnitOfWork uow;
    static UnitOfWorkManagement unitOfWorkManagement;
    static IUnitOfWorkEvents events;

    Establish context = () =>
    {
      mocks = new MockRepository();
      events = mocks.Stub<IUnitOfWorkEvents>();
      unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(events);
      uow = new UnitOfWork(unitOfWorkManagement);
    };

    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Start();
    };

    It should_call_start_for_the_unit_of_work = () =>
      events.AssertWasCalled(x => x.Start(uow));
  }

  [Subject("Rolling back a unit of work")]
  public class when_rolling_back_a_unit_of_work_with_no_events
  {
    static MockRepository mocks;
    static UnitOfWork uow;
    static UnitOfWorkManagement unitOfWorkManagement;
    static object instance;
    static Exception error;

    Establish context = () =>
    {
      mocks = new MockRepository();
      unitOfWorkManagement = new UnitOfWorkManagement();
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

  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_with_no_events
  {
    static MockRepository mocks;
    static UnitOfWork uow;
    static UnitOfWorkManagement unitOfWorkManagement;
    static object instance;
    static Exception error;

    Establish context = () =>
    {
      mocks = new MockRepository();
      unitOfWorkManagement = new UnitOfWorkManagement();
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
      events.AssertWasCalled(x => x.Rollback(uow, added));
      events.AssertWasCalled(x => x.Rollback(uow, saved));
      events.AssertWasCalled(x => x.Rollback(uow, deleted));
    };

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();

    It should_call_rollback_for_the_unit_of_work = () =>
      events.AssertWasCalled(x => x.Rollback(uow));
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
  
  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_with_added_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.AddNew(added);
      uow.Commit();
    };

    It should_call_addnew = () => events.AssertWasCalled(x => x.AddNew(uow, added));

    It should_call_commit_for_the_unit_of_work = () =>
      events.AssertWasCalled(x => x.Commit(uow));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_multiple_times_with_added_objects : with_events_for_committing_and_rolling_back
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
  
  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_with_saved_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Save(added);
      uow.Commit();
    };

    It should_call_save = () => events.AssertWasCalled(x => x.Save(uow, added));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_with_deleted_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Delete(added);
      uow.Commit();
    };

    It should_call_delete = () => events.AssertWasCalled(x => x.Delete(uow, added));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_with_added_and_then_deleted_objects : with_events_for_committing_and_rolling_back
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
      events.AssertWasNotCalled(x => x.AddNew(uow, added));
      events.AssertWasNotCalled(x => x.Save(uow, added));
      events.AssertWasNotCalled(x => x.Delete(uow, added));
    };

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_with_saved_and_then_deleted_objects : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Save(added);
      uow.Delete(added);
      uow.Commit();
    };

    It should_call_delete = () => events.AssertWasCalled(x => x.Delete(uow, added));

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Committing a unit of work")]
  public class when_committing_a_unit_of_work_with_added_and_then_saved_objects : with_events_for_committing_and_rolling_back
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
      events.AssertWasCalled(x => x.AddNew(uow, added));
      events.AssertWasCalled(x => x.Save(uow, added));
    };

    It should_clear_entries = () => uow.Entries.ShouldBeEmpty();
  }
  
  [Subject("Committing a unit of work")]
  public class when_disposing_of_a_unit_of_work : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Save(saved);
      uow.Dispose();
    };

    It should_call_rollback = () =>
    {
      events.AssertWasCalled(x => x.Rollback(uow, saved));
    };

    It should_not_call_committing_events = () =>
    {
      events.AssertWasNotCalled(x => x.Save(uow, saved));
    };
  }
  
  [Subject("Committing a unit of work")]
  public class when_disposing_of_a_committed_unit_of_work : with_events_for_committing_and_rolling_back
  {
    Because of = () =>
    {
      mocks.ReplayAll();
      uow.Save(saved);
      uow.Commit();
      uow.Dispose();
    };

    It should_call_committing_events = () =>
    {
      events.AssertWasCalled(x => x.Save(uow, saved));
    };

    It should_not_call_rollback = () =>
    {
      events.AssertWasNotCalled(x => x.Rollback(uow, saved));
    };
  }
}
