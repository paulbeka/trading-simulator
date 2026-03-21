

# The Plan

We can't fully simulate getting live prices, so we'll fetch from Yahoo Finance the positions that we currently own for equities. 

Workflow:
- User choses to buy/sell assets on a dashboard. They can look them up with a searchbar.
- If they place an order, it's stored somewhere and bought at the price the order is executed.
- That goes into a positions database
- On the dashboard, the user can see their live P&L from different assets. 

Options:
- the user can buy options, which also has a P&L component. 
- The option is priced using black scholes etc (+ maybe using some other methods such as vol smiles to properly model the real prices)
- The option can then be re-sold or executed as necessary


Ok so here's the plan:
 - I now have updates on stock prices published to Kafka
 - Create a PnL consumer
 - At init, create two maps, user->positions and tickers-> user. Populate the initial prices
 - Update prices in memory as new prices come in [long term goal use sharding]
 - For each stock update, update user PnL [could this be partitioned into kafka?]
 - With new user PnL, use kafka to publish to the .NET api section can send a webhook push
 - the webhook push should send an initial format, then only send updates 
 - the updates should include new stock prices which pertain to the user (and include PnL)

Later:
 - Make sure new orders have their own queue, and execute at a certain price and get persisted to both memory and the positions database (which should include when the stock was bought)
 - include valuations for options of different types


 Make sure to calculate unrealised PnL + realised PnL