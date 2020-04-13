Feature: DDD

Scenario: Create a new order
	When I create an order
	Then Order is created

Scenario: Add order line
	Given I have an order
	When I add 1 items of product pepper to the order
	Then Product pepper is added with an quantity of 1

Scenario: Add order line citron
	Given I have an order
	When I add 5 items of product citron to the order
	Then Product citron is added with an quantity of 5

Scenario: Create a new order with an item
	When I create an order
	#And I add 1 items of product pepper to the order
	Then Order is created
	And Product pepper is added with an quantity of 1

Scenario: Remove order line from order
	Given I have an order
	And order has an item product salt whith quantity 1
	When I remove line with identity 1 from order
	Then itemline 1 is removed

Scenario: Change order line quantity
	Given I have an order
	And order has an item product salt whith quantity 1
	When I change quantity to 3 from orderline with id 1
	Then itemline 1 quantity is changed to 3

Scenario: Change order line quantity to same value
	Given I have an order
	And order has an item product salt whith quantity 7
	When I change quantity to 7 from orderline with id 1
	Then No Result is expected

Scenario: Add same product again
	Given I have an order
	And order has an item product salt whith quantity 7
	When I add 3 items of product salt to the order
	Then itemline 1 quantity is changed to 10
